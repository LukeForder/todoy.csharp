/// <reference path="C:\Users\218456\Desktop\todoy\todoy.csharp\Todoy.Web\Scripts/moment.js" />
// namespace boilerplate
var todoy = todoy || {};
todoy.toDo = todoy.toDo || {};
todoy.toDo.filters = todoy.toDo.filters || {};

(function addHumanDateFilterToNamespace(ns) {

    function HumanDateFilter(date) {
        return moment(date).fromNow();
    }

    ns.HumanDateFilter = HumanDateFilter;

})(todoy.toDo.filters);