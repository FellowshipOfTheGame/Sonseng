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
    const baseValue = await price.ref.parent.child('base-value').once('value')
    console.log('base value: ', baseValue)
    await coins.ref.parent.child('bought-powerUps').update({
      [powerUp]: {
        level: 0,
        multiplier: 1,
        baseValue: baseValue.val(),
      },
    })
    const nextUpgrade = await admin
      .database()
      .ref(`/power-ups/${powerUp}/upgrades/1`)
      .once('value')

    return res.send({
      powerUp,
      cogs: coins.val() - price.val(),
      nextPrice: nextUpgrade.exists() ? nextUpgrade.child('price').val() : 0,
      prevMult: 1,
      nextMult: nextUpgrade.exists()
        ? nextUpgrade.child('multiplier').val()
        : 0,
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
  const currentUpgrade = await admin
    .database()
    .ref(`/users/${uid}/bought-powerUps/${powerUp}`)
    .once('value')
  const nextLevel = currentUpgrade.child('level').val() + 1

  const upgrade = await admin
    .database()
    .ref(`power-ups/${powerUp}/upgrades/${nextLevel}`)
    .once('value')

  if (!upgrade.exists()) {
    return res
      .status(401)
      .send({ message: `!${powerUp}! Upgrade nível máximo` })
  }
  if (coins.val() > upgrade.child('price').val()) {
    await coins.ref.parent.update({
      coins: coins.val() - upgrade.child('price').val(),
    })

    await currentUpgrade.ref.update({
      level: nextLevel,
      multiplier: upgrade.child('multiplier').val(),
    })

    const nextUpgrade = await admin
      .database()
      .ref(`/power-ups/${powerUp}/upgrades/${nextLevel + 1}`)
      .once('value')

    if (nextUpgrade.exists()) {
      return res.send({
        powerUp,
        cogs: coins.val() - upgrade.child('price').val(),
        nextPrice: nextUpgrade.child('price').val(),
        nextMult: nextUpgrade.child('multiplier').val(),
        prevMult: currentUpgrade.child('multiplier').val(),
      })
    } else {
      return res.send({
        powerUp,
        cogs: coins.val(),
        nextPrice: -1,
        prevMult: -1,
        nextMult: -1,
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
    const powerUp = await admin
      .database()
      .ref(`/users/${uid}/bought-powerUps/${name}`)
      .once('value')
    if (powerUp.exists()) {
      const next = await admin
        .database()
        .ref(`/power-ups/${name}/upgrades/${powerUp.child('level').val() + 1}`)
        .once('value')

      if (next.exists()) {
        prices.push({
          name,
          price: next.child('price').val(),
          max: false,
          level: powerUp.child('level').val(),
          nextMult: next.child('multiplier').val(),
          baseValue: powerUp.child('baseValue').val(),
          prevMult: powerUp.child('multiplier').val(),
        })
      } else {
        prices.push({
          name,
          price: -1,
          max: true,
          level: powerUp.child('level').val(),
          baseValue: -1,
          prevMult: -1,
          nextMult: -1,
        })
      }
    } else {
      const infos = await admin
        .database()
        .ref(`/power-ups/${name}`)
        .once('value')
      prices.push({
        name,
        price: infos.child('price').val(),
        max: false,
        level: -1,
        baseValue: infos.child('base-value').val(),
        prevMult: -1,
        nextMult: 1,
      })
    }
  })
  Promise.all(childPromises)
    .then(() => {
      return res.send({ prices })
    })
    .catch((err) => {
      return res.status(500).send({ message: err })
    })
})

module.exports = (app) => app.use('/powerup', router)
