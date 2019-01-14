// JavaScript Document
; (function ($) {
    $.fn.tabs = function (options) {
        var defaults = {
            navCell: ".nav li",
            conCell: ".con",
            currCell: "on",
            tabIndex: 0
        };
        var opts = $.extend(defaults, options);
        return this.each(function () {
            var that = $(this),
			btn = that.find(opts.navCell),
			con = that.find(opts.conCell),
			i = opts.tabIndex;
            var navappVer = navigator.userAgent.match(/(iPhone|iPod|Android|ios|iOS|iPad|Backerry|WebOS|Symbian|Windows Phone|Phone)/i);
            var isVarfun = function (vfName) { try { return "undefined" == typeof vfName ? false : "function" == typeof eval(vfName) ? true : true } catch (e) { } return false };
            var isZeJq = isVarfun('Zepto') ? (navappVer ? "tap" : "click") : "click";
            btn.removeClass(opts.currCell).eq(i).addClass(opts.currCell);
            con.children().hide().eq(i).show();
            btn.on(isZeJq, function () {
                i = $(this).index();
                btn.removeClass(opts.currCell).eq(i).addClass(opts.currCell);
                con.children().hide().eq(i).show();
            });
        });
    }
})(window.jQuery || window.Zepto || window.$);