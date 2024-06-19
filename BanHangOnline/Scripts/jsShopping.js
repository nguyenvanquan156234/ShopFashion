$(document).ready(function () {
    ShowCount();

    $('body').on('click', '.btnAddToCart', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        var quantity = 1;
        var tsl = $('#quantity_value').text();
        if (tsl != null && tsl.trim() !== "") {
            quantity = parseInt(tsl);
        }
        $.ajax({
            url: '/ShoppingCart/AddToCart',
            type: 'POST',
            data: { id: id, quantity: quantity },
            success: function (rs) {
                if (rs.Success) {
                    alert(rs.msg);
                    $('#checkout_items').html(rs.Count);
                }
            }
        });
    });

    $('body').on('click', '.btnDelete', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        var conf = confirm('Bạn có muốn xóa sản phẩm này trong giỏ hàng hay không!');
        if (conf) {
            $.ajax({
                url: '/ShoppingCart/Delete',
                type: 'POST',
                data: { id: id },
                success: function (rs) {
                    if (rs.Success) {
                        $('#checkout_items').html(rs.Count);
                        window.location.reload();
                        $('#trow_' + id).remove();
                       
                    }
                }
            });
        }
    });

    $('body').on('click', '.btnDeleteAll', function (e) {
        e.preventDefault();
        var conf = confirm('Bạn có muốn xóa tất cả sản phẩm trong giỏ hàng hay không!');
        if (conf) {
            DeleteAll();
        }
    });
    $('body').on('click', '.btnUpdateAll', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        var quantity = $('#Quantity_' + id).val();
        UpdateAll(id, quantity)
        
    });
    // Handle the change event on the quantity input field
  
});

function ShowCount() {
    $.ajax({
        url: '/ShoppingCart/ShowCount',
        type: 'GET',
        success: function (rs) {
            $('#checkout_items').html(rs.Count);
        }
    });
}

function DeleteAll() {
    $.ajax({
        url: '/ShoppingCart/DeleteAll',
        type: 'POST',
        success: function (rs) {
            if (rs.Success) {
                loadCart();
                location.reload();
                
            }
        }
    });
}
function UpdateAll(id, quantity) {
    $.ajax({
        url: '/ShoppingCart/UpdateAll',
        type: 'POST',
        data: { id: id, quantity: quantity },
        success: function (rs) {
            if (rs.Success) {
                // Update total amount
                $('#totalAmount').text(rs.TotalAmount.toFixed(2));

                // Update quantity and total price for the updated item
                $('#Quantity_' + id).val(quantity);
                var totalPrice = parseFloat(quantity) * parseFloat($('#Price_' + id).text());
                $('#TotalPrice_' + id).text(totalPrice.toFixed(2));

                // Update total price for other items
                $('[id^=TotalPrice_]').each(function () {
                    var itemId = $(this).attr('id').split('_')[1];
                    var itemQuantity = $('#Quantity_' + itemId).val();
                    var itemPrice = $('#Price_' + itemId).text();
                    $(this).text((parseFloat(itemQuantity) * parseFloat(itemPrice)).toFixed(2));
                });
            }
        }
    });
}


function loadCart() {
    $.ajax({
        url: '/ShoppingCart/Partial_Item_Cart',
        type: 'GET',
        success: function (rs) {
            $('#load_data').html(rs);
        }
    });
}