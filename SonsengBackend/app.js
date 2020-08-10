const express = require('express')
const port = 5000
const app = express()
const fs = require('fs')
const bodyParser = require('body-parser')
const privateKey = fs.readFileSync('key.pem', 'utf-8')
const certificate = fs.readFileSync('cert.pem', 'utf-8')
const https = require('https')
const fetch = require('node-fetch')
const credentials = { key: privateKey, cert: certificate }
const firebase = require('firebase')
const passport = require('passport')

app.use((req, res, next) => {
  res.header('Access-Control-Allow-Origin', '*')
  res.header(
    'Access-Control-Allow-Headers',
    'Origin, X-Requested-With, Content-Type, Accept, Authorization'
  )
  if (req.method === 'OPTIONS') {
    res.header('Access-Control-Allow-Methods', 'PUT, POST, PATCH, DELETE, GET')
    return res.status(200).json({})
  }
  next()
})

app.use(bodyParser.urlencoded({ extended: false }))
app.use(bodyParser.json())
app.use(passport.initialize())
const firebaseConfig = {
  apiKey: 'AIzaSyBYy8ShFigxjhrZKAWMYjDqvL5ZS0dT0DM',
  authDomain: 'sonseng2020-1586957105557.firebaseapp.com',
  databaseURL: 'https://sonseng2020-1586957105557.firebaseio.com',
  projectId: 'sonseng2020-1586957105557',
  storageBucket: 'sonseng2020-1586957105557.appspot.com',
  messagingSenderId: '321646647310',
  appId: '1:321646647310:web:d21a8045f7c3b96fbbc45f',
  measurementId: 'G-1PWQPC720F',
}
firebase.initializeApp(firebaseConfig)
const database = firebase.database()
const auth = firebase.auth()
const httpsServer = https.createServer(credentials, app)
httpsServer.listen(port, () => {
  console.log(`Listening at ${port}`)
})

const GoogleStrategy = require('passport-google-oauth').OAuth2Strategy

passport.serializeUser(function (user, done) {
  done(null, user)
})

passport.deserializeUser(function (user, done) {
  done(null, user)
})

passport.use(
  new GoogleStrategy(
    {
      clientID:
        '321646647310-693rmelrk5re3cm29f2jc969n6t2mnpd.apps.googleusercontent.com',
      clientSecret: 'Q2kzdxMxSzh79yKwtSOCnyAl',
      callbackURL: 'https://localhost:5000/auth/google/callback',
    },
    function (accessToken, refreshToken, profile, done) {
      const credential = firebase.auth.GoogleAuthProvider.credential(
        null,
        accessToken
      )
      auth.signInWithCredential(credential).catch((err) => {
        console.log('Firebase auth sign in err ', err)
      })
      return done(null, { user: profile })
    }
  )
)

app.get(
  '/auth/google',
  passport.authenticate('google', {
    scope: ['https://www.googleapis.com/auth/plus.login'],
  })
)

app.get(
  '/auth/google/callback',
  passport.authenticate('google'),
  (req, res) => {
    console.log(req.user)
    res.send(req.user)
  }
)
