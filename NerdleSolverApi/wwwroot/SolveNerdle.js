await fetch('https://code.jquery.com/jquery-3.7.1.min.js')
    .then(response => {
        if (!response.ok) {
            throw new Error('Failed to load script');
        }
        return response.text();
    })
    .then(scriptText => {
        const scriptElement = document.createElement('script');
        scriptElement.textContent = scriptText;
        document.body.appendChild(scriptElement);
    })
    .catch(error => {
        console.error('Error loading script:', error);
    });

function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}

async function onSuccess(response) {
    var theGuess = response.guess;
    for (var i = 0; i < theGuess.length; i++) {
        var c = theGuess[i];
        if (c == '+') {
            $('button').find('#plustitle').closest('button').click();
        }
        else if (c == '-') {
            $('button').find('#minustitle').closest('button').click();
        }
        else if (c == '*') {
            $('button').find('#timestitle').closest('button').click();
        }
        else if (c == '/') {
            $('button').find('#divtitle').closest('button').click();
        }
        else {
            var buttonText = 'button:contains("' + theGuess[i] + '")';
            $(buttonText).click();
        }
        await sleep(70);
    }
    $('button:contains("Enter")').click();

    if (response.count == 1) {
        console.log("Finished solving!")
        return;
    }

    var state = getGameState();

    var postBody = [];
    for (var r = 0; r < state.length; r++) {
        var guess = "";
        var result = "";

        for (var i = 0; i < state[r].length; i++) {
            var parts = state[r][i].split(" ")
            guess += parts[0];
            result += parts[1][0].toUpperCase();
        }

        postBody.push({
            guess: guess,
            result: result
        });
    }

    doPost(postBody);
}

function getGameState(){
    var pbGrid = $('.pb-grid');
    var values = []

    for (var row = 0; row < 6; row++) {
        var theRow = []
        for (var col = 0; col < 8; col++) {
            theRow.push(pbGrid.children()[row].children[col].getAttribute('aria-label'))
        }
        if (!theRow[0].includes('undefined')) {
            values.push(theRow)
        }
    }

    return values
}

function doPost(postBody) {
    $.ajax({
        url: "http://localhost:5209/suggest-guess",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(postBody),
        success: onSuccess,
        error: function (xhr, status, error) {
            console.error("Error:", error);
        }
    });
}

doPost([]);