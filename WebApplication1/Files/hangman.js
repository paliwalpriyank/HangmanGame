(function () {
    "use strict";
    //defines all the variables to be used to maintain user state.
    var availableLetters, words, guessInput, guess, guessButton, resetButton, lettersGuessed, lettersMatched, output, man, letters,
        lives, currentWord, numLettersMatched, messages, masterData, count, noOfSpaces, score, charList, alphaGrid, category,
        categoryselected, totalscore, user, myscore;
    // get all the data based on category selected by the user. 
    // This method used jquery ajax call to get the data in json format.
    function getData()
    {
        var query = window.location.search.substring(1);
        var url = 'http://localhost:17476/ProductRESTService.svc/GetProductList/';
        var vars = query.split("&");
        var vars1=vars[0].split("=");
        categoryselected=vars1[1];
        $.ajax({
            url: 'http://localhost:17476/ProductRESTService.svc/GetProductList/'+categoryselected,
            type: 'GET',
            dataType: 'json',
            accepts: { json: 'application/json; charset=utf-8' },
            success: function (data) {
                masterData = data;
                displayWord();
            },

            error: function () {
                alert('Kya');
            }
        })
        var query1 = window.location.search.substring(1);
        var varsuser = query1.split("&");
        var vars1user = varsuser[2].split("=");
        user=vars1user[1];
        $.ajax({
            url: 'http://localhost:17476/ProductRESTService.svc/GetScores/' + user,
            type: 'GET',
            dataType: 'json',
            accepts: { json: 'application/json; charset=utf-8' },
            success: function (data) {
                totalscore=data
            },

            error: function () {
                alert('Kya');
            }
        })
        
    }
    // This method initiate the HTML 5 canvas element and create basic images like pole and hanging rope
    function createCanvas(){
        var c = document.getElementById("myCanvas");
        var ctx = c.getContext("2d");
        ctx.beginPath();
        ctx.fillStyle = "#00000";

        ctx.fillRect(520, 20, 15, 430);
        ctx.fillRect(180, 20, 340, 10);
        ctx.moveTo(300, 20);
        ctx.lineTo(300, 120);


        ctx.moveTo(450, 20);
        ctx.lineTo(525, 120);
        ctx.stroke();
        c = document.getElementById("myCanvas");
        ctx = c.getContext("2d");
        var my_gradient = ctx.createLinearGradient(130, 450, 130, 500);
        my_gradient.addColorStop(0, "brown");
        my_gradient.addColorStop(1, "white");
        ctx.fillStyle = my_gradient;
        ctx.fillRect(130, 450, 430, 50);
        
    }
    //This method is the entry point of the hangman game and initiates all other methods.
    // It do work like setting up counter to zero. Defining messages etc.
    function setup() {
        /* start config options */       
        getData();
        availableLetters = "abcdefghijklmnopqrstuvwxyz";
        charList = availableLetters.split("");
        
        count = 0;
        myscore = 0;
        messages = {
            win: 'You win!',
            lose: 'Game over!',
            guessed: ' already guessed, please try again...',
            validLetter: 'Please enter a letter from A-Z'
        };
        /* end config options */

        

        /* make #man and #output blank, create vars for later access */
        output = document.getElementById("output");
        man = document.getElementById("man");
        category = document.getElementById("category");
        resetButton = document.getElementById("restart");
        score = document.getElementById("score");
        alphaGrid = document.getElementById("alphaGrid");
        for (var i = 0; i < charList.length; i++) {
            if (i % 6 == 0)
                $("body .alphaGrid").append('<br/>')
            var r = $('<input type="button" value="'+charList[i].toUpperCase()+'" name="'+charList[i]+'"/>');
            $("body .alphaGrid").append(r);
            
        }
    }
    // This method display the word on the scree. This is called every time a new word is to be displayed on screen.
    function displayWord() {
        $('.alphaGrid').show()
        var c = document.getElementById("myCanvas");
        var ctx = c.getContext("2d");
        ctx.clearRect(0, 0, 600, 500);
        createCanvas();
        lives = 5;
        lettersGuessed = lettersMatched = '';
        numLettersMatched = 0;

        /* choose a word */
        //currentWord = words[Math.floor(Math.random() * words.length)];
        currentWord = masterData[count].FilmName.trim();
        man.innerHTML = 'You have ' + lives + ' lives remaining';
        score.innerHTML = '<span style="float:left;"><b>Total Score: </b>' + totalscore + '</span>' + '<span style="float:right;"><img style="width:30px;height:30px;" src="image/cookie.jpg"></span><span style="float:right;"><b>Current Score: </b>' + myscore + '</span>';
            
        category.innerHTML = '<b>Category: </b>' + categoryselected.toUpperCase();
        output.innerHTML = '';
        noOfSpaces=(currentWord.match(/ /g)||[]).length;
       

        /* make sure guess button is enabled */
        
        
       

        /* set up display of letters in current word */
        letters = document.getElementById("letters");
        letters.innerHTML = '<li class="current-word">Current word:</li>';

        var letter, i;
        for (i = 0; i < currentWord.length; i++) {
            letter = '<li class="letter letter' + currentWord.charAt(i).toUpperCase() + '">' + currentWord.charAt(i).toUpperCase() + '</li>';
            letters.insertAdjacentHTML('beforeend', letter);
        }
    }
    // This method evaluates the game for win or loss condition.
    function gameOver(win) {
        if (win) {
            output.innerHTML = messages.win;
            output.classList.add('win');
            count++;
            myscore++;
            displayWord();
        } else {
            output.innerHTML = messages.lose+"      Correct Movie Was "+currentWord;
            output.classList.add('error');
            var c = document.getElementById("myCanvas");
            var ctx = c.getContext("2d");
            ctx.font = "30px Georgia";
            ctx.fillStyle = "Red";
            ctx.fillText("GAME OVER", 215, 400);
            $('.alphaGrid').hide();
            count++;
        }

        //guessInput.style.display = guessButton.style.display = 'none';
       // guessInput.value = '';

    }
   
    /* Start game - should ideally check for existing functions attached to window.onload */
    window.onload = setup();
    
    /* buttons */
    
    // jquery method is called on exit button clicked.
    $('#exit').click(function (e) {
        
        $.ajax({
            url: 'http://localhost:17476/ProductRESTService.svc/AddScores/' + myscore + '/' + user,
            type: 'GET',
            dataType: 'json',
            accepts: { json: 'application/json; charset=utf-8' },
            success: function (data) {
                window.location.assign("\Exit.html");
            },

            error: function () {
                alert('Some Error Occured');
            }
        })
    });
    
  
    // this is called on reset call.
    resetButton.onclick = function () {
        displayWord();
    };
    // this button is called when alphabet grid is clicked and evaluated and updates UI.
    $('.alphaGrid input').click(function(e)
    {
        if (e.preventDefault) e.preventDefault();
        output.innerHTML = '';
        output.classList.remove('error', 'warning');
        guess = this.name;

        /* does guess have a value? if yes continue, if no, error */
        if (guess) {
            /* is guess a valid letter? if so carry on, else error */
            if (availableLetters.indexOf(guess) > -1) {
                /* has it been guessed (missed or matched) already? if so, abandon & add notice */
                if ((lettersMatched && lettersMatched.indexOf(guess) > -1) || (lettersGuessed && lettersGuessed.indexOf(guess) > -1)) {
                    output.innerHTML = '"' + guess.toUpperCase() + '"' + messages.guessed;
                    output.classList.add("warning");
                }
                /* does guess exist in current word? if so, add to letters already matched, if final letter added, game over with win message */
                else if (currentWord.toLowerCase().indexOf(guess) > -1) {
                    var lettersToShow;
                    lettersToShow = document.querySelectorAll(".letter" + guess.toUpperCase());

                    for (var i = 0; i < lettersToShow.length; i++) {
                        lettersToShow[i].classList.add("correct");
                    }

                    /* check to see if letter appears multiple times */
                    for (var j = 0; j < currentWord.length; j++) {
                        if (currentWord.toLowerCase().charAt(j) === guess) {
                            numLettersMatched += 1;
                        }
                    }

                    lettersMatched += guess;
                    if (numLettersMatched === currentWord.length - noOfSpaces) {
                        gameOver(true);
                    }
                }
                /* guess doesn't exist in current word and hasn't been guessed before, add to lettersGuessed, reduce lives & update user */
                else {
                	var c = document.getElementById("myCanvas");
                	var ctx=c.getContext("2d");
                	ctx.lineWidth="6";
                	ctx.beginPath();
                	if(lives==5)
                		{
                			ctx.arc(300,155,35,0,2*Math.PI);
                		}
                	else if(lives==4)
                		{                		
                		ctx.moveTo(300,190);
                		ctx.lineTo(300,320);
                		}
                	else if(lives==3)
            		{
                		ctx.moveTo(300,210);
                		ctx.lineTo(260,235);
            		}
                	else if(lives==2)
            		{
                		ctx.moveTo(300,210);
                		ctx.lineTo(340,235);
            		}
                	else if(lives==1)
            		{
                		ctx.moveTo(300,320);
                		ctx.lineTo(260,350);
                		ctx.moveTo(300,320);
                		ctx.lineTo(340,350);
            		}
                	
                	
                	ctx.stroke();
                	
                    lettersGuessed += guess;
                    lives--;
                    man.innerHTML = 'You have ' + lives + ' lives remaining';
                    if (lives === 0) gameOver();
                }
            }
            /* not a valid letter, error */
            else {
            	
                output.classList.add('error');
                output.innerHTML = messages.validLetter;
            }
        }
        /* no letter entered, error */
        else {
            output.classList.add('error');
            output.innerHTML = messages.validLetter;
        }
        return false;
    });
}());