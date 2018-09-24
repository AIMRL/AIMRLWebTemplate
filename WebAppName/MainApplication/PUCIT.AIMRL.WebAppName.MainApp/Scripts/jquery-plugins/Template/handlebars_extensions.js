Handlebars.registerHelper('ifNotEqual', function (v1, v2, options) {
    if (v1 != v2) {
        return options.fn(this);
    }
    return options.inverse(this);
});

Handlebars.registerHelper('ifEqual', function (v1, v2, options) {
    if (v1 == v2) {
        return options.fn(this);
    }
    return options.inverse(this);
});

Handlebars.registerHelper('ifGreaterThan', function (v1, v2, options) {
    if (v1 > v2) {
        return options.fn(this);
    }
    return options.inverse(this);
});

Handlebars.registerHelper('ifLessThan', function (v1, v2, options) {
    if (v1 < v2) {
        return options.fn(this);
    }
    return options.inverse(this);
});