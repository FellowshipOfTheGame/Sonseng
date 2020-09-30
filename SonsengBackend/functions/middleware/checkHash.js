const admin = require('../controller/fb')
const crypto = require('crypto')

module.exports = (req, res, next)=>{
    const {score, cogs, highestScore,hash} = req.body
    const part = req.headers.authorization.match(/\.(.*?)\./)
    let checkHash = crypto.createHash('sha256').update(`saiHackerFedido${score}${highestScore}${cogs}${part[0]}`).digest('hex')
    if(checkHash !== hash) return res.status(403).send({message:'Hoje não hasquer fedido!'})
    next()
}