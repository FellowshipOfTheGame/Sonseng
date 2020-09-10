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
    if (bought.exists()) {
      return res.status(500).send({ message: 'Você já possui este power up!' })
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
      const nextPrice = await admin
        .database()
        .ref(`power-ups/${powerUp}/upgrades/1/price`)
        .once('value')
      return res.send({ powerUp, cogs: newCoin, nextPrice: nextPrice.val() })
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
    const nextLevel = powerUpLevel.val() + 1

    const price = await admin
      .database()
      .ref(`power-ups/${powerUp}/upgrades/${nextLevel}/price`)
      .once('value')

    if (!price.exists()) {
      return res.status(404).send({ message: 'Upgrade máximo!' })
    }
    if (coins.val() > price.val()) {
      await coins.ref.parent.update({ coins: coins.val() - price.val() })

      const newMultiplier = await price.ref.parent
        .child('multiplier')
        .once('value')

      await powerUpLevel.ref.parent.update({
        level: nextLevel,
        multiplier: newMultiplier.val(),
      })

      const nextPrice = await powerUpLevel.ref.parent
        .child(`/upgrades/${nextLevel}/price`)
        .once('value')

      return res.send({
        powerUp,
        cogs: coins.val() - price.val(),
        nextPrice: nextPrice.exists() ? nextPrice.val() : 0,
      })
    } else {
      return res
        .status(500)
        .send({ message: 'Você não tem engrenagens suficientes!' })
    }
  } else {
    return res.send('Ora ora ora um hasker!')
  }
})

exports.getCurrentPrice = functions.https.onRequest(async (req, res) => {
  if (req.method === 'POST') {
    const { uid, powerUp } = req.body
    const powerUpLevel = await admin
      .database()
      .ref(`/users/${uid}/bought-powerUps/${powerUp}/level`)
      .once('value')

    if (powerUpLevel.exists()) {
      const price = await admin
        .database()
        .ref(`/power-ups/${powerUp}/upgrades/${powerUpLevel.val() + 1}/price`)
        .once('value')

      if (price.exists()) {
        return res.send({ powerUp, price: price.val() })
      } else {
        return res.status(404).send({ message: 'Upgrade nível máximo' })
      }
    }
    const basePrice = await admin
      .database()
      .ref(`/power-ups/${powerUp}/price`)
      .once('value')
    return res.send({ powerUp, price: basePrice.val() })
  } else {
    return res.send('Nononoo I see what you did there')
  }
})
