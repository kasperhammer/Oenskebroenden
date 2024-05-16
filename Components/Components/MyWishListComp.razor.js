





export async function GetColor(id) {

    if (id != 0) {
        await new Promise(r => setTimeout(r, 5));
        return $("#" + id).css("background-color");
    }
    /*    $("#"+id)*/
}

export async function ToogleToolTip(id, show) {
    console.log(id, show);
    if (show) {
     
        $("#Wish_" + id).children().first().children().first().children().first().css("bottom","10vw");
        $("#Wish_" + id).children().first().children().last().css("display", "block");
/*        $("Wish_" + id).children().last().show();*/

    } else {
        $("#Wish_" + id).children().first().children().first().children().first().css("bottom", "0vw");
        $("#Wish_" + id).children().first().children().last().css("display", "none");
        //$("Wish_" + id).children().first().show();
        //$("Wish_" + id).children().last().hide();

    }
}
export function DisplayLink(link) {
    alert(link);
}