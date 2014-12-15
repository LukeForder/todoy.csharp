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

    ToDoListController.prototype.addToDo = function addToDo() {

        var self = this;

        self.toDoList.push(self.newToDo);

        self.newToDo = new todoy.toDo.models.ToDoItem();
    };

    ToDoListController.prototype.complete = function complete(toDo) {
        var self = this;
        toDo.done = true;
    };

    ns.ToDoListController = ToDoListController;

})(todoy.toDo.controllers);