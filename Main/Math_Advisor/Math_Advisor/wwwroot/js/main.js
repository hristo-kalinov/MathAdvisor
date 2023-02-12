function replacePow(expression) {
    // Define a regular expression to match pow(x, y)
    var regex = /pow\(\s*(\w+)\s*,\s*([^\)]+)\s*\)/g;

    // Replace instances of pow(x, y) with (x)^(y)
    expression = expression.replace(regex, "$1^$2");

    // Repeat the replace process until no more instances of pow are found
    while (expression.match(regex)) {
        expression = expression.replace(regex, "$1^$2");
    }

    return expression;
}
//span handling
var mathFieldSpan = document.getElementById('math-field');
//var latexSpan = document.getElementById('latex');
//var stringSpan = document.getElementById('string');
var MQ = MathQuill.getInterface(2); // for backcompat
//var mathFieldContainer = $("#math-field-container");
// Get the original dimensions of the child element
//var originalWidth = mathFieldSpan.width;
//var originalHeight = mathFieldSpan.height;
var publicMathString = "";
var mathField = MQ.MathField(mathFieldSpan, {
    spaceBehavesLikeTab: true, // configurable
    handlers: {

        edit: function () { // useful event handlers
            let mathString = mathField.latex();
            mathString = mathString.replaceAll("\\cdot", "*");
            mathString = latex_to_js(mathString);

            mathString = replacePow(mathString);
            publicMathString = mathString;
        }
    }
});

//button handling

var plusButton = $("#plus-button");
var minusButton = $("#minus-button");
var multButton = $("#mult-button");
var divButton = $("#div-button");
var powButton = $("#pow-button");
var sqrtButton = $("#sqrt-button");
var piButton = $("#pi-button");

plusButton.on("click", function () {
    mathField.cmd('+');
});
minusButton.on("click", function () {
    mathField.cmd("-");
});
multButton.on("click", function () {
    mathField.cmd("*");
});
divButton.on("click", function () {
    mathField.cmd("/");
});
powButton.on("click", function () {
    mathField.cmd("^");
});
sqrtButton.on("click", function () {
    mathField.cmd("\\sqrt");
});
piButton.on("click", function () {
    mathField.cmd("π");
});

//send button handling
