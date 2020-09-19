const express = require('express')
const router = express.Router()
const admin = require('./fb')
const verifyMiddleWare = require('../middleware/verifyFirebaseToken')

router.use(verifyMiddleWare)

router.post('/saveStats', async (req, res) => {
  const { uid, score, cogs } = req.body
  const user = await admin.database().ref(`/users/${uid}`).once('value')
  const scoreNumber = Number.parseInt(score)
  let highestScore = user.child('highest-score')
  if(highestScore.exists()){
    highestScore = Number.parseInt(highestScore.val())
  }else{
    highestScore = 0
  }
  try {
    if (scoreNumber > highestScore) {
      user.ref.update({
        'highest-score': Number.parseInt(score),
        'last-score': scoreNumber,
      })
      highestScore = scoreNumber
    } else user.ref.update({ 'last-score': scoreNumber })
    let coins = user.child('coins')
    if (coins.exists()) {
      coins = Number.parseInt(coins.val())
      console.log(coins)
      coins += Number.parseInt(cogs)
      await user.ref.update({ coins: coins })
      return res.send({ highestScore, cogs: coins })
    } else {
      await user.ref.update({ coins: Number.parseInt(cogs) })
      return res.send({ highestScore, cogs: 0 })
    }
  } catch (err) {
    console.log(err)
    return res.status(403).send({ message: err })
  }
})

module.exports = (app) => app.use('/user', router)
