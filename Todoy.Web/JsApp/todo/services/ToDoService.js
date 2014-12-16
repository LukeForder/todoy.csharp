// namespace boilerplate
var todoy = todoy || {};
todoy.toDo = todoy.toDo || {};
todoy.toDo.services = todoy.toDo.services || {};

(function addToDoServiceToNamespace(ns) {

    function ToDoService(httpService, qService, siteUrl) {
        this.httpService = httpService;
        this.qService = qService;
        this.siteUrl = siteUrl;
    }

    ToDoService.prototype.addAsync = function AddAsync(toDo) {

        function onAddedToDo(toDo) {
            task.resolve(toDo);
        }

        function onErrorAddingToDo(reasons, status) {
            console.log("error adding toDo %o, %i", reasons, status);
            task.reject(reasons);
        }

        var self = this;

        var task = self.httpService.defer();

        var toDoDto = {
            Details: toDo.Details
        };
        
        self.httpService.post(self.siteUrl + '/api/todo', toDoDto).
        success(onAddedToDo).
        error(onErrorAddingToDo);

        return task.promise;
    }

    ns.ToDoService = ToDoService;

})(todoy.toDo.controllers);