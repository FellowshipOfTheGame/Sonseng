mergeInto(LibraryManager.library, {
  SignInWithGoogle: function (objectName, callback, fallback) {
    var parsedObjectName = Pointer_stringify(objectName)
    var parsedCallback = Pointer_stringify(callback)
    var parsedFallback = Pointer_stringify(fallback)

    try {
      var provider = new firebase.auth.GoogleAuthProvider()
      firebase
        .auth()
        .signInWithPopup(provider)
        .then(function (result) {
          result.user
            .getIdToken(false)
            .then(function (token) {
              unityInstance.Module.SendMessage(
                parsedObjectName,
                parsedCallback,
                token
              )
            })
            .catch(function (error) {
              unityInstance.Module.SendMessage(
                parsedObjectName,
                parsedFallback,
                JSON.stringify(error, Object.getOwnPropertyNames(error))
              )
            })
        })
        .catch(function (error) {
          unityInstance.Module.SendMessage(
            parsedObjectName,
            parsedFallback,
            JSON.stringify(error, Object.getOwnPropertyNames(error))
          )
        })
    } catch (error) {
      unityInstance.Module.SendMessage(
        parsedObjectName,
        parsedFallback,
        JSON.stringify(error, Object.getOwnPropertyNames(error))
      )
    }
  },
})