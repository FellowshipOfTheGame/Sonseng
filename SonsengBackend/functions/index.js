const functions = require('firebase-functions')
const admin = require('firebase-admin')
admin.initializeApp()

exports.purchasePowerUp = functions.https.onRequest(async (req, res) => {
  if (req.method === 'POST') {
    const { uid, powerUp } = req.body
    const bought = await admin
      .database()
      .ref(`/users/${uid}/bought-powerUps/${powerUp}`)
      .once('value')
    if(bought.exists()){
      return res.status(500).send({message:'Você já possui este power up!'})
    }
    const coins = await admin
      .database()
      .ref(`/users/${uid}/coins`)
      .once('value')
    const price = await admin
      .database()
      .ref(`power-ups/${powerUp}/price`)
      .once('value')
    const newCoin = coins.val() - price.val()

    if (newCoin >= 0) {
      await coins.ref.parent.update({ coins: newCoin })
      await coins.ref.parent.child('bought-powerUps').update({
        [powerUp]: {
          level: 0,
          multiplier: 1,
        },
      })
      return res.send({ cogs: newCoin })
    } else {
      return res
        .status(500)
        .send({ message: 'Você não tem engrenagens suficientes!' })
    }
  } else {
    return res.send('Ora ora ora um hasker!')
  }
})

exports.purchaseUpgrade = functions.https.onRequest(async (req, res) => {
  if (req.method === 'POST') {
    const { uid, powerUp } = req.body
    const coins = await admin
      .database()
      .ref(`/users/${uid}/coins`)
      .once('value')
    const powerUpLevel = await admin
      .database()
      .ref(`/users/${uid}/bought-powerUps/${powerUp}/level`)
      .once('value')
    let levelNum = -1
    if (powerUpLevel.val() !== 0) levelNum++

    const price = await admin
      .database()
      .ref(`power-ups/${powerUp}/upgrades/${levelNum + 1}/price`)
      .once('value')
    if (coins.val() > price.val()) {
      await coins.ref.parent.update({ coins: coins.val() - price.val() })
      const newMultiplier = await price.ref.parent
        .child('multiplier')
        .once('value')
      await powerUpLevel.ref.parent.update({
        level: levelNum + 1,
        multiplier: newMultiplier.val(),
      })
      return res.send({ coins: coins.val() - price.val() })
    } else {
      return res
        .status(500)
        .send({ message: 'Você não tem engrenagens suficientes!' })
    }
  } else {
    return res.send('Ora ora ora um hasker!')
  }
})
