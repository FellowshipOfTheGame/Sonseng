const express = require('express')
const cookieParser = require('cookie-parser')()
const cors = require('cors')
const app = express()
const functions = require('firebase-functions')
const bodyParser = require('body-parser')

require('./controller/leaderboardController')(app)
require('./controller/powerUpController')(app)
require('./controller/userController')(app)

app.use(
  cors({
    origin: [
      'https://danbarretto.itch.io/tira-a-tampa',
      'https://127.0.0.1',
      'https://v6p9d9t4.ssl.hwcdn.net/',
    ],
  })
)
app.use(cookieParser)
app.use(bodyParser)

exports.app = functions.https.onRequest(app)
