/* 舊有的 Js 控制區塊 */
$(document).ready(function () {
    var $drawer = $('.drawer')
    var $toggleDrawerButton = $('.toggle-drawer-button')
    var $toggleAllDrawerButton = $('.toggle-all-drawer-button')

    $drawer.on('open', function (e) {
        e.stopPropagation()
        var $self = $(this)
        var $directTitle = $($self.find('.drawer__title')[0])
        var $directContent = $($self.find('.drawer__content')[0])
        var $directToggleDrawerButton = $($self.find('.toggle-drawer-button')[0])
        showContent()

        function showContent() {
            $self.addClass('drawer--shown')
            $directToggleDrawerButton.html('<i class="fas fa-minus"></i>收起')
            $directTitle.addClass('drawer__title--shown')
            $directContent.show()
        }
    })
    $drawer.on('close', function (e) {
        e.stopPropagation()
        var $self = $(this)
        var $directTitle = $($self.find('.drawer__title')[0])
        var $directContent = $($self.find('.drawer__content')[0])
        var $directToggleDrawerButton = $($self.find('.toggle-drawer-button')[0])
        hideContent()

        function hideContent() {
            $self.removeClass('drawer--shown')
            $directToggleDrawerButton.html('<i class="fas fa-plus"></i>展開')
            $directTitle.removeClass('drawer__title--shown')
            $directContent.hide()
        }
    })

    $toggleDrawerButton.on('click', function () {
        var $self = $(this)
        var $directParent = $($self.parents('.drawer')[0])
        var $shownDrawer = []

        if ($directParent.hasClass('drawer--shown')) {
            $directParent.trigger('close')
        } else {
            $directParent.trigger('open')
        }

        $shownDrawer = $('.drawer--shown')
        if ($shownDrawer.length === 0) {
            $toggleAllDrawerButton.trigger('toOpenTrigger')
        } else {
            $toggleAllDrawerButton.trigger('toCloseTrigger')
        }
    })

    $toggleAllDrawerButton.on('click', function () {
        var $self = $(this)
        var $drawer = $('.drawer')
        var currentStatus = $self.data('current-status')
        if (currentStatus === 'open') {
            $drawer.trigger('close')
            $self.trigger('toOpenTrigger')
        } else {
            $drawer.trigger('open')
            $self.trigger('toCloseTrigger')
        }
        return false;
    })
    $toggleAllDrawerButton.on('toOpenTrigger', function () {
        var $self = $(this)
        $self.html('<i class="fas fa-plus"></i>全部展開')
        $self.data('current-status', 'close')
    })
    $toggleAllDrawerButton.on('toCloseTrigger', function () {
        var $self = $(this)
        $self.html('<i class="fas fa-minus"></i>全部收合')
        $self.data('current-status', 'open')
    })
})