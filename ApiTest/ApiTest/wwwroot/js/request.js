function AddElements(input) {
    // Get the explanation div
    var explanation = document.getElementById("explanation");
    while (explanation.firstChild) {
        explanation.removeChild(explanation.firstChild);
    }
    // Split the string into separate steps
    if (input.endsWith("\n")) {
        input = input.slice(0, -1);
      }
    var steps = input.split("\n");

    // Counter for the step number
    var stepCounter = 1;

    // Add the first part of the explanation as an equation in MathQuill
    latex = StringToLaTeX(steps[0]);
    

    var firstPart = document.createElement("div");
    firstPart.setAttribute("class", "mathquill-static-text");
    var equation = MQ.StaticMath(firstPart);
    equation.latex(latex);

    // Wrap the first part in a div
    var firstPartWrapper = document.createElement("div");
    firstPartWrapper.style.fontSize = "22px";
    firstPartWrapper.appendChild(firstPart);
    explanation.appendChild(firstPartWrapper);

    // Loop through each step starting from the second one
    for (var i = 1; i < steps.length; i++) {
        // Split the step into text and equation
        var parts = steps[i].split(":");

        // Extract the text and equation
        var text = parts[0].trim();
        var equation = parts[1].trim();
        equation = equation.replace(/\s+/g, '')
        // Create a new step
        var step = document.createElement("div");

        // Add the step number
        var stepNumber = document.createElement("div");
        stepNumber.setAttribute("class", "step-number");
        stepNumber.innerHTML = "Step " + stepCounter + ": ";
        step.appendChild(stepNumber);

        // Add the text
        var stepText = document.createElement("div");
        stepText.innerHTML = text;
        stepText.setAttribute("class", "text-static");
        step.appendChild(stepText);

        // Create a div to hold the equation
        var stepEquationWrapper = document.createElement("div");
        stepEquationWrapper.style.fontSize = "22px";

        // Add the equation as a MathQuill static object

        var latex = StringToLaTeX(equation)
        var stepEquation = document.createElement("div");
        stepEquation.setAttribute("class", "mathquill-static-text");
        var equation = MQ.StaticMath(stepEquation);
        equation.latex(latex);

        // Add the stepEquationWrapper to the step
        stepEquationWrapper.appendChild(stepEquation);
        step.appendChild(stepEquationWrapper);

        // Add the step to the explanation div
        explanation.appendChild(step);

        // Increment the step counter
        stepCounter++;
    }
}

function StringToLaTeX(equation)
{
    var sides = equation.split("=");
    if (sides.length > 1)
    {
        var lhs = math.parse(sides[0]);
        var rhs = math.parse(sides[1]);
        var lhsTex = lhs.toTex().replace(/\\left\(/g, '').replace(/\\right\)/g, '');
        var rhsTex = rhs.toTex().replace(/\\left\(/g, '').replace(/\\right\)/g, '');
        var latex = lhsTex + "=" + rhsTex;
    }
    else
    {
        var lhs = math.parse(sides[0]);
        var latex = lhs.toTex().replace(/\\left\(/g, '').replace(/\\right\)/g, '');
    }
    return latex
}

function AddAnswers(answers){
    var explanation = document.getElementById("explanation");
    var answersEl= document.createElement("div");
    answersEl.setAttribute("class", "step-number");
    answersEl.innerHTML = "Answers";
    explanation.appendChild(answersEl);
    if (answers.length < 2 || answers[0] === answers[1])
    {
        var latex = StringToLaTeX("x="+answers[0]);
        var answer = document.createElement("div");
        answer.setAttribute("class", "mathquill-static-text");
        var equation = MQ.StaticMath(answer);
        equation.latex(latex);

        // Wrap the first part in a div
        var wrapper = document.createElement("div");
        wrapper.style.fontSize = "22px";
        wrapper.appendChild(answer);
        explanation.appendChild(wrapper);
    }
    else{
        for(var i = 0; i < answers.length; i++){
            var latex = StringToLaTeX("x_"+(i+1).toString() + "=" + answers[i])
            var answer = document.createElement("div");
            answer.setAttribute("class", "mathquill-static-text");
            var equation = MQ.StaticMath(answer);
            equation.latex(latex);

            // Wrap the first part in a div
            var wrapper = document.createElement("div");
            wrapper.style.fontSize = "22px";
            wrapper.appendChild(answer);
            explanation.appendChild(wrapper);
        }
    }
}
document.getElementById("math-field").addEventListener("keydown", function (event) {
    if (event.key === "Enter") {
        sendData();
    }
});

document.getElementById("send-button").addEventListener("click", sendData);

function sendData() {
    console.log(publicMathString)
    fetch('https://localhost:7251/GetSolution', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(publicMathString)
    })

        .then(response => response.text())
        .then(responseText => {
            console.log(responseText)
            jsonObj = JSON.parse(responseText);
            if (!jsonObj.success)
            {
                var explanation = document.getElementById("explanation");
                while (explanation.firstChild) {
                    explanation.removeChild(explanation.firstChild);
                }
                var answersEl= document.createElement("div");
                answersEl.setAttribute("class", "step-number");
                answersEl.innerHTML = "We can't solve this type of equation or there was a syntax error. Please try again.";
                explanation.appendChild(answersEl);
                return;
            }
            AddElements(jsonObj.solutionString);

            AddAnswers(jsonObj.answers)
            
        });
}