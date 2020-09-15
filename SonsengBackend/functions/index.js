const express = require('express')
const cookieParser = require('cookie-parser')()
const cors = require('cors')({ origin: true })
const app = express()
const functions = require('firebase-functions')
const bodyParser = require('body-parser')

require('./controller/leaderboardController')(app)
require('./controller/powerUpController')(app)

app.use(cors)
app.use(cookieParser)
app.use(bodyParser)

exports.app = functions.https.onRequest(app)