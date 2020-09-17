const admin = require('firebase-admin')
const functions = require('firebase-functions')

module.exports = admin.initializeApp(functions.config().firebase)
