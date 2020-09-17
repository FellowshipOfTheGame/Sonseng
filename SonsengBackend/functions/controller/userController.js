const express = require('express')
const router = express.Router()
const admin = require('./fb')
const verifyMiddleWare = require('../middleware/verifyFirebaseToken')

router.use(verifyMiddleWare)

router.post('/saveStats', async (req, res) => {
  const { uid, score, cogs } = req.body
  const user = await admin.database().ref(`/users/${uid}`).once('value')

  let highestScore = user.child('highest-score').val()
  try {
    if (score > highestScore) {
      user.ref.update({ 'highest-score': score, 'last-score': score })
      highestScore = score
    } else user.ref.update({ 'last-score': score })
    let coins = user.child('coins').val()
    await user.ref.update({ coins: coins + cogs })
    coins += cogs
    return res.send({ highestScore, cogs: coins })
  } catch (err) {
    return res.status(403).send({ message: err })
  }
})

router.post('/getBoughtUpgrades', async (req, res) => {
  const { uid } = req.body

  const upgrades = await admin
    .database()
    .ref(`/users/${uid}/bought-powerUps`)
    .once('value')
  if (upgrades.exists()) {
    let children = []
    upgrades.forEach((child) => {
      children.push(child)
    })

    let powerUpInfo = []
    const childPromises = children.map(async (up) => {
      const name = up.key
      const baseValue = await admin
        .database()
        .ref(`power-ups/${name}/base-value`)
        .once('value')
      powerUpInfo.push({
        key: name,
        baseValue,
        level: up.child('level').val(),
        multiplier: up.child('multiplier').val(),
      })
    })
    await Promise.all(childPromises).catch((err) => {
      return res.send(500).send({ message: err })
    })
    return res.send({ powerUps: powerUpInfo })
  }
  return res.status(404).send({ message: 'Você não comprou nenhum power up!' })
})

module.exports = (app) => app.use('/user', router)
