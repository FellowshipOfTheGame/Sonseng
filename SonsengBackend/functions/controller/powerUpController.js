const express = require('express')
const router = express.Router()
const admin = require('./fb')
const verifyMiddleWare = require('../middleware/verifyFirebaseToken')

router.use(verifyMiddleWare)

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
    if (nextPrice.exists()) {
      return res.send({
        powerUp,
        cogs: coins.val() - price.val(),
        nextPrice: nextPrice.val(),
      })
    } else {
      return res.send({
        powerUp,
        cogs: coins.val(),
        nextPrice: -1,
      })
    }
  } else {
    return res
      .status(401)
      .send({ message: 'Você não tem engrenagens suficientes!' })
  }
})

router.post('/getAllPrices', async (req, res) => {
  const { uid } = req.body
  const powerUps = await admin.database().ref(`/power-ups/`).once('value')
  let prices = []
  let children = []
  powerUps.forEach((child) => {
    children.push(child)
  })

  const childPromises = children.map(async (child) => {
    const name = child.key
    const powerUpLevel = await admin
      .database()
      .ref(`/users/${uid}/bought-powerUps/${name}/level`)
      .once('value')
    console.log(name, powerUpLevel.exists())
    if (powerUpLevel.exists()) {
      const price = await admin
        .database()
        .ref(`/power-ups/${name}/upgrades/${powerUpLevel.val() + 1}/price`)
        .once('value')

      if (price.exists()) {
        prices.push({
          name,
          price: price.val(),
          max: false,
          level: powerUpLevel.val(),
        })
      } else {
        prices.push({ name, price: -1, max: true, level: powerUpLevel.val() })
      }
    } else {
      const price = await admin
        .database()
        .ref(`/power-ups/${name}/price`)
        .once('value')
      prices.push({ name, price: price.val(), max: false, level: 0 })
    }
  })
  await Promise.all(childPromises).catch((err) => {
    return res.status(500).send({ message: err })
  })
  return res.send({ prices })
})

module.exports = (app) => app.use('/powerup', router)
