

$(function () {
    console.log("ready!");
});

export async function GetColor(id) {
    
    if (id != 0) {
        await new Promise(r => setTimeout(r, 5));
        return $("#" + id).css("background-color");
    }
/*    $("#"+id)*/
}

