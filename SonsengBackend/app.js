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

const firebaseConfig = {
 /*
  Paste configs here
 */
}
firebase.initializeApp(firebaseConfig)
const database = firebase.database()

function writeUserData(userName, userScore){
  database.ref('users/'+userName).set({
    name:userName,
    score:userScore
  })
}


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

app.use(bodyParser.urlencoded({extended:false}))
app.use(bodyParser.json())


app.post('/test', (req, res)=>{
  writeUserData(req.body.userName, req.body.userScore)
  res.sendStatus(200)
})

app.post('/getScore', (req, res)=>{
  database.ref('/users/'+req.body.userName).once('value').then((snapshot)=>{
    const userScore = (snapshot.val() && snapshot.val().score)
    res.send(userScore)
  })
})

const httpsServer = https.createServer(credentials, app)
httpsServer.listen(port, () => {
  console.log(`Listening at ${port}`)
})
