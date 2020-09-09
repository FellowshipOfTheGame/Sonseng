const functions = require('firebase-functions')
const admin = require('firebase-admin')
admin.initializeApp()

// Create and Deploy Your First Cloud Functions
// // https://firebase.google.com/docs/functions/write-firebase-functions

exports.buyPowerUp = functions.database
  .ref('/users/{uid}/bought-powerUps/{powerUp}')
  .onCreate(async (snapshot, context) => {
    const uid = context.params.uid
    const powerUp = context.params.powerUp
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
        return snapshot.ref.update({ level: 0, multiplier: 1 })
    }
    return null
  })

exports.buyUpgrade = functions.database
  .ref('/users/{uid}/bought-powerUps/{powerUp}')
  .onUpdate((change, context) => {
    const uid = context.params.uid
    const powerUp = context.params.powerUp
    const before = change.before.val()
    const after = change.after.val()
    if (after === true) {
        return null
    }else{
        console.log("agr vai")
    }
  })
