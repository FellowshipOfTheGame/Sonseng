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
    let coins = Number.parseInt(user.child('coins').val())
    await user.ref.update({ coins: coins + Number.parseInt(cogs) })
    coins += Number.parseInt(cogs)
    return res.send({ highestScore, cogs: coins })
  } catch (err) {
    console.log(err)
    return res.status(403).send({ message: err })
  }
})


module.exports = (app) => app.use('/user', router)
