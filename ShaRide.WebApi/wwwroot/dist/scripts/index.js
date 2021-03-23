// active detail
headers = document.querySelectorAll('.who h4');
descriptions = document.querySelectorAll('.detailContainer .description');
for (let i = 0; i < headers.length; i++) {
    headers[i].addEventListener("mouseover", function() {
        var activeHeader = document.querySelector(".who .activeHeader");
        activeHeader.classList.remove("activeHeader");
        this.classList.add("activeHeader");
        var activeDescription = document.querySelector(".detailContainer .activeDescription");
        activeDescription.classList.remove("activeDescription");
        descriptions[i].classList.add("activeDescription");
    });
}


//active advantages
advantageText = document.querySelectorAll('#advantages .text .description');
advantageImg = document.querySelectorAll('#advantages img');
for (let i = 0; i < advantageText.length; i++) {
    advantageText[i].addEventListener("mouseover", function() {
        var activeText = document.querySelector('#advantages .text .activeText')
        activeText.classList.remove("activeText");
        advantageText[i].classList.add("activeText");
        var activeImg = document.querySelector('#advantages .activeImg');
        activeImg.classList.remove("activeImg");
        advantageImg[i].classList.add("activeImg");
    })
}

//scroll to join section
document.querySelector('.menu button').addEventListener('click', function() {

    var bannerHeight = document.getElementsByClassName('banner')[0].clientHeight;
    if (document.querySelector('nav').clientHeight == 80) {
        var windowHeight = bannerHeight - 80;
    } else {
        var windowHeight = bannerHeight - 60;
    }
    window.scrollTo({
        top: windowHeight,
        behavior: 'smooth',
    })
});

//menu toogle
document.getElementsByClassName('menuIcon')[0].addEventListener('click', function() {
    var menu = document.querySelector('nav').classList.toggle('open');
})

document.querySelector('nav .menu').addEventListener('click', function() {
    var menu = document.querySelector('nav').classList.remove('open');
})

//send number
var send = document.querySelector('#bonus .send');
var sending = document.querySelector('#bonus .sending');
var sent = document.querySelector('#bonus .sent');
send.addEventListener("click", function() {
    var phone = document.getElementById('phone').value;
    var regex = /\+\d{3}\s(50|51|55|60|70|77|99)\s-\s(\d{3})\s-\s(\d{2})\s-\s(\d{2})/;
    if (regex.test(phone)) {
        document.querySelector('#bonus input').classList.remove('incorrect');
        document.querySelector('#bonus .alertText').style.display = 'none'
        
        // switching button animation to on.
        sending.style.display = 'block';
        send.style.display = 'none';

        var getUrl = window.location;
        var baseUrl = getUrl .protocol + "//" + getUrl.host + "/" + getUrl.pathname.split('/')[1];

        //sending post request to our account controller
        $.ajax({
            url:baseUrl+'InsertPotentialClient',
            type:"POST",
            data:{ phone: phone },
            dataType:"json",
            success: function(){
                // switching button animation to off.
                sending.style.display = 'none';
                sent.style.display = 'block';
            }
        })
        
    } else {
        document.querySelector('#bonus input').classList.add('incorrect');
        document.querySelector('#bonus .alertText').style.display = 'block'
    }
})



//phone mask regex
document.getElementById('phone').addEventListener('focus', function(e) {
    e.target.value = "+994";
});

document.getElementById('phone').addEventListener('input', function(e) {
    var x = e.target.value.replace(/\D/g, '').match(/\d{0,3}(\d{0,2})(\d{0,3})(\d{0,2})(\d{0,2})/);
    e.target.value = '+994 ' + (!x[2] ? x[1] : x[1] + ' - ' + x[2] + (x[3] ? ' - ' + x[3] : '') + (x[4] ? ' - ' + x[4] : ''));

});

var descriptionHeader = document.querySelectorAll('#detailList .description .header');
var openDescription = document.querySelectorAll('#detailList .description .down');
var closeDescription = document.querySelectorAll('#detailList .description .up');
var detailDescriptionText = document.querySelectorAll('#detailList .description .text');
for (let i = 0; i < descriptionHeader.length; i++) {
    descriptionHeader[i].addEventListener('click', function() {
        detailDescriptionText[i].classList.toggle('open');
        closeDescription[i].classList.toggle('active');
        openDescription[i].classList.toggle('active');
    })
}