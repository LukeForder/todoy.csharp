// namespace boilerplate
var todoy = todoy || {};
todoy.authentication = todoy.authentication || {};
todoy.authentication.services = todoy.authentication.services || {};

(function addIdentityServiceToNamespace(ns) {

    var dbKey = "_authorization";

    function IdentityService(cookieStore) {

        Object.defineProperty(
            this,
            'authorizationToken',
            {
                get: function () {
                    return cookieStore.get(dbKey);
                },
                set: function (value) {
                    cookieStore.put(dbKey, value);
                }
            });
    }
   
    ns.IdentityService = IdentityService;

})(todoy.authentication.services);