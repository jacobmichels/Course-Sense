"use strict";

// import './style.css';
import 'bootstrap/dist/js/bootstrap.min.js';
import 'bootstrap/dist/css/bootstrap.min.css';

import '@fortawesome/fontawesome-free/js/fontawesome'
import '@fortawesome/fontawesome-free/js/brands'

import $ from 'jquery';
import 'jquery-mask-plugin';

let subjectList;

let requestState = false;

//object to keep track of state of validity for user input 
let validState = new Map(Object.entries({
    phone:false,
    email:false,
    subject:false,
    code:false,
    section:false,
    term:false
}));

$(document).ready(()=>{
    initializeMasks();
    populateSubjectsArray();
    setValidStateMap();
});

window.validateEmail=function(email,indicate){
    //regex taken from https://emailregex.com/
    const re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
    if(re.test(email)){
        if(indicate){
            $(event.target).removeClass("is-invalid");
            console.log("valid");
        }
        validState.email=true;
        return true;
    }
    if(indicate){
        $(event.target).addClass("is-invalid");
        console.log("invalid");
    }
    validState.email=false;
    return false;
};

window.validatePhone = function(phone,indicate){
    //regex taken from EeeeeK and marty at https://stackoverflow.com/questions/4338267/validate-phone-number-with-javascript
    const re = /^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$/im;
    if(re.test(phone)){
        if(indicate){
            $(event.target).removeClass("is-invalid");
            console.log("valid ph");
        }
        validState.phone=true;
        return true;
    }
    if(indicate){
        $(event.target).addClass("is-invalid");
        console.log("invalid ph");
    }
    validState.phone=false;
    return false;
};

window.validateSubject = function(subject,indicate){
    if(subject.length!==3&&subject.length!==4){
        if(indicate){
            $(event.target).addClass("is-invalid");
            console.log("invalid subject");
        }
        validState.subject=false;
        return false;
    }
    if(indicate){
        $(event.target).removeClass("is-invalid");
        console.log("valid subject");
    }
    validState.subject=true;
    return true;
};

window.validateCode = function(code,indicate){
    if(code.length!==4){
        if(indicate){
            $(event.target).addClass("is-invalid");
            console.log("invalid code");
        }
        validState.code=false;
        return false;
    }
    if(indicate){
        $(event.target).removeClass("is-invalid");
        console.log("valid code");
    }
    validState.code=true;
    return true;
};

window.validateSection = function(section,indicate){
    if(section.length>4||section.length<2){
        if(indicate){
            $(event.target).addClass("is-invalid");
            console.log("invalid section");
        }
        validState.section=false;
        return false;
    }
    if(indicate){
        $(event.target).removeClass("is-invalid");
        console.log("valid section");
    }
    validState.section=true;
    return true;
}

window.validateTerm = function(term,indicate){
    if(term.length!==3){
        if(indicate){
            $(event.target).addClass("is-invalid");
            console.log("invalid term");
        }
        validState.term=false;
        return false;
    }
    if(indicate){
        $(event.target).removeClass("is-invalid");
        console.log("valid term");
    }
    validState.term=true;
    return true;
}

function populateSubjectsArray(){
    const Http = new XMLHttpRequest();
    const url = '/subjects';
    Http.open("GET",url);
    Http.send();
    Http.onreadystatechange=function(){
        if(this.readyState===4&&this.status===200){
            // console.log(JSON.parse(Http.responseText));
            subjectList=JSON.parse(Http.responseText);
            console.log("populated subjects list");
        }
        else if(this.readyState===4&&this.status!==200){
            console.log("subject list population failed");
        }
    }
};

function initializeMasks(){
    $('#phone-input').mask('(000) 000-0000');

    //taken from johnathansantos at https://github.com/igorescobar/jQuery-Mask-Plugin/issues/210
    $('#subject-input').mask('0000', {
        'translation': {
            0: {pattern: /[A-Za-z]/}
        },
        onKeyPress: function (value, event) {
            event.currentTarget.value = value.toUpperCase();
        }
    });
    $('#code-input').mask('0000');
    $("#section-input").mask('AAAA');
    $('#term-input').mask('A00', {
        'translation': {
            A: {pattern: /[A-Za-z]/}
        },
        onKeyPress: function (value, event) {
            event.currentTarget.value = value.toUpperCase();
        }
    });
}

let userInput={};

window.modalPrompt = function(){
    userInput.phone = $('#phone-input').val();
    userInput.email = $('#email-input').val();
    userInput.code = $('#code-input').val();
    userInput.subject = $('#subject-input').val();
    userInput.section = $('#section-input').val();
    userInput.term = $('#term-input').val();
    
    if(!validState.email&&!validState.phone){
        return console.log("must provide at least one valid method of notification (email/ph)");
    }

    if(!allCourseInputValid()){
        return console.log("at least one course input field invalid, correct and resubmit");
    }
    let modalContent = "You are about to send a request to be notified of an opening in the course <strong>{0}</strong>. You will be notified through your <strong>{1}</strong>. Does this look good?<span id=\"modal-status\"></span>"
    let newContent = modalContent.replace('{0}',userInput.subject+' '+userInput.code+' section '+userInput.section+' in the term '+userInput.term);
    let contactString="";
    if(validState.email && validState.phone){
        contactString = contactString +' email: '+userInput.email+' and texted at the number: '+userInput.phone;
    }
    else if (validState.email){
        contactString = contactString +' email: '+userInput.email+'.'
    }
    else if (validState.phone){
        contactString = contactString +' a text message to the number: '+userInput.phone+'.'
    }
    newContent = newContent.replace('{1}',contactString);
    $('#modal-body').html(newContent);
    $('#submit-modal').modal("toggle");
}

window.submitNotificationRequest = function(){
    //user input already validated, simply send the request
    // $('#main-modal-close').tooltip();
    $('.spinner').removeAttr("hidden","hidden");
    $('.modal-close-btn').attr("disabled","disabled");
    $('#modal-submit-btn').attr("disabled","disabled");
    requestState=true;

    const Http = new XMLHttpRequest();
    const url = '/notificationrequest';
    Http.open("POST",url);
    Http.setRequestHeader('Content-Type','application/json');
    Http.send(JSON.stringify({
        "RequestedCourse": {
            "Code": userInput.code,
            "Subject": userInput.subject,
            "Section": userInput.section,
            "Term": userInput.term
        },
        "Phone":userInput.phone,
        "Email":userInput.email,
    }));
    Http.onreadystatechange=function(){
        if(this.readyState===4&&this.status===200){
            console.log("notification good");
            requestState=false;
            $('#modal-status').html('<br><br>Success! You will get a notification when a spot opens :)');
            $('#modal-submit-btn').removeAttr("disabled","disabled");
            $('.modal-close-btn').removeAttr("disabled","disabled");
            $('.spinner').attr("hidden","hidden");
            
        }
        else if(this.readyState===4&&this.status!==200){
            if(this.responseText==='Bad course'){
                $('#modal-status').html('<br><br>The course that you entered does not exist. Please try again.');
                console.log("Bad course");
            }
            else if(this.responseText==='Bad contact'){
                $('#modal-status').html('<br><br>Some of the contact info you entered did not pass server validation. Please go back and fix any errors.');
                console.log("Bad contact");
            }
            else {
                $('#modal-status').html("<br><br>Internal server error, please try again. If this keeps happening, please send me an email and I'll look into it");
                console.log("Internal server error");
            }
            requestState=false;
            $('#modal-submit-btn').removeAttr("disabled","disabled");
            $('.modal-close-btn').removeAttr("disabled","disabled");
            $('.spinner').attr("hidden","hidden");
        }
    }
}

function allCourseInputValid(){
    if(validState.subject && validState.code && validState.term && validState.section){
        return true;
    }
    return false;
}

function setValidStateMap(){
    window.validateCode($('#code-input').val(),false);
    window.validateEmail($('#email-input').val(),false);
    window.validatePhone($('#phone-input').val(),false);
    window.validateSection($('#section-input').val(),false);
    window.validateTerm($('#term-input').val(),false);
    window.validateSubject($('#subject-input').val(),false);
}

window.closeModal = function(){
    if(!requestState){
        $('#submit-modal').modal("toggle");
    }
    else{
        console.log("Cannot close, waiting for response from server.");
        let err = ('<br><br>This window cannot be closed while request is pending. Please wait for the server to respond.');
        $('#modal-status').html(err);
    }
}