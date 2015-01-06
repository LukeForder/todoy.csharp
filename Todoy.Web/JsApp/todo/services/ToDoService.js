// namespace boilerplate
var todoy = todoy || {};
todoy.toDo = todoy.toDo || {};
todoy.toDo.services = todoy.toDo.services || {};

(function addToDoServiceToNamespace(ns) {

    function ToDoService(httpService, qService, siteUrl, authenticationService) {
        this.httpService = httpService;
        this.qService = qService;
        this.siteUrl = siteUrl;
    }

    function asModel(dto) {

        var model = new todoy.toDo.models.ToDoItem();

        model.details = dto.Details;
        model.date = dto.CreatedDate;
        model.done = !!dto.DoneDate;
        model.id = dto.Id;

        return model;
    }

    ToDoService.prototype.addAsync = function AddAsync(toDo) {

        function onAddedToDo(dto) {

            var model = asModel(dto);

            console.log(model);

            task.resolve(model);
        }

        function onErrorAddingToDo(dto, status) {
            console.log("error adding toDo %o, %i", dto.Errors, status);
            task.reject(dto.Errors);
        }

        var self = this;

        var task = self.qService.defer();

        var toDoDto = {
            Details: toDo.details
        };

        self.httpService.post(self.siteUrl + '/api/todo', toDoDto).
        success(onAddedToDo).
        error(onErrorAddingToDo);

        return task.promise;
    };

    ToDoService.prototype.getAllAsync = function getAllAsync() {

        function onGotAll(dtos) {
            var models =  dtos.map(
                function (dto) {
                    return asModel(dto);
                });

            task.resolve(models);
        }

        function onFailedToGetAll(response, status) {
            task.reject(response.Errors);
        }
        
        var self = this;

        var task = self.qService.defer();

        self.httpService.get(self.siteUrl + '/api/todo').
           success(onGotAll).
           error(onFailedToGetAll);

        return task.promise;
    };

    ToDoService.prototype.completeToDoAsync = function completeToDoAsync(todo) {
        
        function onToDoMarkedAsComplete(dto) {
            task.resolve(dto);
        }

        function onErrorMarkingToDoAsComplete(response, status) {
            task.reject(response.Errors);
        }

        var self = this;

        var task = self.qService.defer();

        var url = self.siteUrl + '/api/todo/' + todo.id + '/completed';

        self.httpService.patch(
            url,
            null).
        success(onToDoMarkedAsComplete).
        error(onErrorMarkingToDoAsComplete);

        return task.promise;

    };

    ns.ToDoService = ToDoService;

})(todoy.toDo.services);