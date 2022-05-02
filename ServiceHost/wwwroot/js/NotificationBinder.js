

function $handleNotificationRead(notificationId) {
    var table = $('#main-table').DataTable();
    connection.invoke("MarkRead", notificationId).then(() => {

        $('#main-table tbody').on('click', 'button', function () {
            table.row($(this).parents('tr'))
                .remove()
                .draw();
        });
        invokeNotification();
        invokeCountUnreadNotifications();

    }).catch(function (err) {
        return console.error(err.toString());
    });
}

