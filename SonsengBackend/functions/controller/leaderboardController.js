const express = require('express')
const router = express.Router()
const admin = require('./fb')

router.get('/getLeaders', async (req, res) => {
  let leadersArray = []
  admin
    .database()
    .ref('users')
    .orderByChild('highest-score')
    .on('value', async (snapshot) => {
      const leaders = snapshot.toJSON()
      await Promise.all(
        Object.keys(leaders).map(async (leaderUid) => {
          const user = await admin.auth().getUser(leaderUid)
          const score = leaders[leaderUid]['highest-score']
          const leader = {
            name: user.displayName !== undefined ? user.displayName : 'Convidado',
            highestScore: score !== undefined ? score : 0,
          }
          leadersArray.push(leader)
        })
      )
      return res.send({ leaders: leadersArray })
    })
})

module.exports = (app) => app.use('/leaderboard', router)
