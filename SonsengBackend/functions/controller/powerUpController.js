const express = require('express')
const router = express.Router()
const admin = require('./fb')
const verifyMiddleware = require('../middleware/verifyFirebaseToken')

router.use(verifyMiddleware)

router.post('/purchasePowerUp', async (req, res) => {
  const { uid, powerUp } = req.body
  const bought = await admin
    .database()
    .ref(`/users/${uid}/bought-powerUps/${powerUp}`)
    .once('value')
  if (bought.exists()) {
    return res.status(401).send({ message: 'Você já possui este power up!' })
  }
  const coins = await admin.database().ref(`/users/${uid}/coins`).once('value')
  const price = await admin
    .database()
    .ref(`power-ups/${powerUp}/price`)
    .once('value')

  if (coins.val() >= price.val()) {
    await coins.ref.parent.update({ coins: coins.val() - price.val() })
    await coins.ref.parent.child('bought-powerUps').update({
      [powerUp]: {
        level: 0,
        multiplier: 1,
      },
    })
    const nextPrice = await admin
      .database()
      .ref(`/power-ups/${powerUp}/upgrades/1/price`)
      .once('value')

    return res.send({
      powerUp,
      cogs: coins.val() - price.val(),
      nextPrice: nextPrice.exists() ? nextPrice.val() : 0,
    })
  } else {
    return res
      .status(401)
      .send({ message: 'Você não tem engrenagens suficientes!' })
  }
})

router.post('/purchaseUpgrade', async (req, res) => {
  const { uid, powerUp } = req.body
  const coins = await admin.database().ref(`/users/${uid}/coins`).once('value')
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
    return res
      .status(401)
      .send({ message: `!${powerUp}! Upgrade nível máximo` })
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

    const nextPrice = await admin
      .database()
      .ref(`/power-ups/${powerUp}/upgrades/${nextLevel + 1}/price`)
      .once('value')
    if (nextPrice.exists())
      return res.send({
        powerUp,
        cogs: coins.val() - price.val(),
        nextPrice: nextPrice.val(),
      })
    else
      return res
        .status(401)
        .send({ message: `!${powerUp}! Upgrade nível máximo` })
  } else {
    return res
      .status(401)
      .send({ message: 'Você não tem engrenagens suficientes!' })
  }
})

router.post('/getCurrentPrice', async (req, res) => {
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
      return res
        .status(401)
        .send({ message: `!${powerUp}! Upgrade nível máximo` })
    }
  }
  const basePrice = await admin
    .database()
    .ref(`/power-ups/${powerUp}/price`)
    .once('value')
  return res.send({ powerUp, price: basePrice.val() })
})

module.exports = (app) => app.use('/powerup', router)