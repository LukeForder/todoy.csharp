// namespace boilerplate
var todoy = todoy || {};
todoy.toDo = todoy.toDo || {};
todoy.toDo.models = todoy.toDo.models || {};

(function addToDoItemToNamespace(ns) {

    function ToDoItem() {
        this.details = null;
        this.date = null;
        this.done = false;
    }
    
    ns.ToDoItem = ToDoItem;

})(todoy.toDo.models);