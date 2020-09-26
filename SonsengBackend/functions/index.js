const express = require('express')
const cookieParser = require('cookie-parser')()
const cors = require('cors')
const app = express()
const functions = require('firebase-functions')
const bodyParser = require('body-parser')
const admin = require('./controller/fb')

app.use(cors({origin:true}))
app.options('*', cors({origin:true}))

app.use(cookieParser)
app.use(bodyParser.json())
app.use(bodyParser.urlencoded({
    extended:true
}))

require('./controller/leaderboardController')(app)
require('./controller/powerUpController')(app)
require('./controller/userController')(app)
exports.app = functions.https.onRequest(app)

exports.initUser = functions.auth.user().onCreate((user)=>{
    return admin.database().ref(`users/${user.uid}/`).update({
        coins:0,
        'highest-score':0,
        'last-score':0,
        'bought-powerUps':{
            shield:{
                level:0,
                baseValue:10,
                multiplier:1
            }
        }
    })
})