// namespace boilerplate
var todoy = todoy || {};
todoy.toDo = todoy.toDo || {};
todoy.toDo.controllers = todoy.toDo.controllers || {};

(function addToDoControllerToNamespace(ns) {

    function ToDoListController(toDoService) {

        this.toDoService = toDoService;

        this.showDoneToDos = false;
        this.toDoList = [];
        this.newToDo = new todoy.toDo.models.ToDoItem();
        this.flash = null;

        this.getAll();
    }

    ToDoListController.prototype.canAddToDo = function canAddToDo() {
        return !!this.newToDo.details;
    };

    ToDoListController.prototype.visibleToDos = function visibleToDos() {

        function toDoIsNotCompleted(toDo) {
            return !toDo.done;
        }

        return this.showDoneToDos ? this.toDoList : this.toDoList.filter(toDoIsNotCompleted);
    };

    ToDoListController.prototype.getAll = function getAll() {

        var self = this;

        function onFetchedAll(dto) {
            console.log(dto);
            self.toDoList = dto;
        }

        function onFailedToFetchAll(response, status) {
            // TODO: display the error
        }

        self.toDoService.getAllAsync().then(onFetchedAll, onFailedToFetchAll);
    };

    ToDoListController.prototype.addToDo = function addToDo() {

        var self = this;

        function onAddedToDo() {
            self.toDoList.push(self.newToDo);
            self.newToDo = new todoy.toDo.models.ToDoItem();
        }

        function failedToAddToDo() {
            // TODO: display the error notification
        }

        self.toDoService.
            addAsync(self.newToDo).
            then(onAddedToDo, failedToAddToDo);
    };

    ToDoListController.prototype.complete = function complete(toDo) {
        var self = this;

        function onCompleted() {
            toDo.done = true;
        }

        function onErrorCompleting() {
            // TODO: show a message
        }

        console.log(toDo);

        self.
            toDoService.
            completeToDoAsync(toDo).
            then(onCompleted, onErrorCompleting);

    };

    ns.ToDoListController = ToDoListController;

})(todoy.toDo.controllers);