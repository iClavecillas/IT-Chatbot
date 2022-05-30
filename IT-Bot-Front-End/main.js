
//   chat button toggler 
var btn_switch  = 1;   

$(".chat-btn").click(function(){
    if (btn_switch == 1){
        $('.chat-popup').toggle(100);    
        $("#toast").css("visibility", "hidden");
        $('.chat-popup').addClass('show');
        $('.chat-btn').html("<i class='fa-regular fa-xmark' style='font-size: 30px;'></i>")
        /*$('.chat-btn').css("visibility","hidden");*/
        btn_switch = 0;
    }else if (btn_switch  == 0){
       $('.chat-popup').toggle(100);  
        $('.chat-popup').removeClass('show');
        $('.chat-btn').html("<i class='fak fa-chatbot' style='font-size: 30px;'></i>");
        btn_switch = 1;     
    }
});

$("#chat-minimize").click(function(){
   $('.chat-popup').toggle(100);  
   $('.chat-popup').removeClass('show');
   $('.chat-btn').html("<i class='fak fa-chatbot' style='font-size: 30px;'></i>");
   btn_switch = 1;  
   /*$('.chat-btn').css("visibility","visible");*/
});

$(document).ready(function(){
        $("#liveToast").toast('show');
        $("#liveToast").toast({ autohide: true });
        $("#liveToast").toast({ delay: 10000 });

        $(".chat-btn").click(function(){
            $("#liveToast").toast('hide');
        });
    });
        

$(document).on({'click': function(){
        $(".webchat__basic-transcript__activity-body").css("display","none");
        $(".webchat__suggested-actions__flow-box").css("display","none");
        $(".webchat__send-box-text-box__input").attr("placeholder","Type 'Hi' to start the conversation");
    }
}, '#chat-reset');

$(document).ready(function(){
    $(".webchat__send-box-text-box__input").change(function(){
        $(".webchat__send-box-text-box__input").attr("placeholder","Type your message");

    });
    $(document).on('keypress',function(e) {
    if(e.which == 13) {
        $(".webchat__send-box-text-box__input").attr("placeholder","Type your message");
    }
});

});