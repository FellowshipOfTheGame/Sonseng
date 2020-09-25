const express = require('express')
const cookieParser = require('cookie-parser')()
const cors = require('cors')
const app = express()
const functions = require('firebase-functions')
const bodyParser = require('body-parser')

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
