function approveComment(id) {
    $.ajax(
        {
            url: "/Admin/Comments/ApproveComment",
            data: { commentId: id },
            type: "PATCH",
            success: function (result) {
                Swal.fire({
                    icon: 'success',
                    text: 'عملیات با موفقیت انجام شد',
                    timer: 2000,
                    position: 'top-end',
                    didOpen: () => {
                        Swal.showLoading()
                        //const b = Swal.getHtmlContainer().querySelector('b')
                        timerInterval = setInterval(() => {
                            //b.textContent = Swal.getTimerLeft()
                        }, 100)
                    },
                    willClose: () => {
                        clearInterval(timerInterval)
                    }
                })
                    .then((result) => { location.reload(); })
            },
            error: function (xhr, status, error) {
                Swal.fire({
                    icon: 'error',
                    title: status,
                    text: error,
                    timer: 2000,
                    timerProgressBar: true,
                    position: 'top-end',
                    didOpen: () => {
                        Swal.showLoading()
                        //const b = Swal.getHtmlContainer().querySelector('b')
                        timerInterval = setInterval(() => {
                            //b.textContent = Swal.getTimerLeft()
                        }, 100)
                    },
                    willClose: () => {
                        clearInterval(timerInterval)
                    }
                })
                    .then((result) => { location.reload(); })
            }
        });
    return 0;
}

function rejectComment(id) {
    $.ajax(
        {
            url: "/Admin/Comments/RejectComment",
            data: { commentId: id },
            type: "PATCH",
            success: function (result) {
                Swal.fire({
                    icon: 'success',
                    text: 'عملیات با موفقیت انجام شد',
                    timer: 2000,
                    position: 'top-end',
                    didOpen: () => {
                        Swal.showLoading()
                        //const b = Swal.getHtmlContainer().querySelector('b')
                        timerInterval = setInterval(() => {
                            //b.textContent = Swal.getTimerLeft()
                        }, 100)
                    },
                    willClose: () => {
                        clearInterval(timerInterval)
                    }
                })
                    .then((result) => { location.reload(); })
            },
            error: function (xhr, status, error) {
                Swal.fire({
                    icon: 'error',
                    title: status,
                    text: error,
                    timer: 2000,
                    timerProgressBar: true,
                    position: 'top-end',
                    didOpen: () => {
                        Swal.showLoading()
                        //const b = Swal.getHtmlContainer().querySelector('b')
                        timerInterval = setInterval(() => {
                            //b.textContent = Swal.getTimerLeft()
                        }, 100)
                    },
                    willClose: () => {
                        clearInterval(timerInterval)
                    }
                })
                    .then((result) => { location.reload(); })
            }
        });
    return 0;
}

function deleteComment(id) {
    $.ajax(
        {
            url: "/Admin/Comments/Delete",
            data: { commentId: id },
            type: "DELETE",
            success: function (result) {
                Swal.fire({
                    icon: 'success',
                    text: 'عملیات با موفقیت انجام شد',
                    timer: 2000,
                    position: 'top-end',
                    didOpen: () => {
                        Swal.showLoading()
                        //const b = Swal.getHtmlContainer().querySelector('b')
                        timerInterval = setInterval(() => {
                            //b.textContent = Swal.getTimerLeft()
                        }, 100)
                    },
                    willClose: () => {
                        clearInterval(timerInterval)
                    }
                })
                    .then((result) => { location.reload(); })
            },
            error: function (xhr, status, error) {
                Swal.fire({
                    icon: 'error',
                    title: status,
                    text: error,
                    timer: 2000,
                    timerProgressBar: true,
                    position: 'top-end',
                    didOpen: () => {
                        Swal.showLoading()
                        //const b = Swal.getHtmlContainer().querySelector('b')
                        timerInterval = setInterval(() => {
                            //b.textContent = Swal.getTimerLeft()
                        }, 100)
                    },
                    willClose: () => {
                        clearInterval(timerInterval)
                    }
                })
                    .then((result) => { location.reload(); })
            }
        });
    return 0;
}
