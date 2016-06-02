Array.prototype.contains = function (e) {
    for (i = 0; i < this.length && this[i] != e; i++);
    return !(i == this.length);
}
global = {
    buildin_names: ['buildin_names', 'define', 'show'],
    define: function (name, value) {
        if (this.buildin_names.contains(name)) {
            console.warn('与系统名字冲突:' + name);
            return;
        }
        if (!(name in global))
            global[name] = value;
        else
            console.warn('冲突的全局变量:' + name);
    },
    show: function () {
        console.info('全局变量：');
        for (var name in global) {
            if (this.buildin_names.contains(name))
                continue;
            console.info(name + ' : ' + global[name]);
        }
    }
};
//global.define("Flag", true);
//console.log(global.Flag);
//global.define("Flag", false);
//global.show();

