var HobbyClue = function () { };

HobbyClue = {
    isRTL: false,
    
    handleDatePickers: function (container) {
        if (jQuery().datepicker) {
            $(container).find('.date-picker').datepicker({
                rtl: false,
                orientation: "left",
                autoclose: true,
                format: 'mm/dd/yyyy'
            });
           $('body').removeClass("modal-open"); // fix bug when inline picker is used in modal
        }
    },

    handleTimePickers: function (container) {

        if (jQuery().timepicker) {
            $(container).find('.timepicker-default').timepicker({
                autoclose: true,
                showSeconds: true,
                minuteStep: 1
            });

            $(container).find('.timepicker-24').timepicker({
                autoclose: true,
                minuteStep: 1,
                showSeconds: true,
                showMeridian: false
            });
        }
    },

    handleDateRangePickers: function (container) {
        if (!jQuery().daterangepicker) {
            return;
        }

        $(container).find('#defaultrange').daterangepicker({
            opens: (HobbyClue.isRTL ? 'left' : 'right'),
            format: 'MM/DD/YYYY',
            separator: ' to ',
            startDate: moment().subtract('days', 29),
            endDate: moment(),
            minDate: '01/01/2012',
            maxDate: '12/31/2014',
        },
            function (start, end) {
                console.log("Callback has been called!");
                $('#defaultrange input').val(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'));
            }
        );

        $(container).find('#reportrange').daterangepicker({
            opens: (HobbyClue.isRTL ? 'left' : 'right'),
            startDate: moment().subtract('days', 29),
            endDate: moment(),
            minDate: '01/01/2012',
            maxDate: '12/31/2014',
            dateLimit: {
                days: 60
            },
            showDropdowns: true,
            showWeekNumbers: true,
            timePicker: false,
            timePickerIncrement: 1,
            timePicker12Hour: true,
            ranges: {
                'Today': [moment(), moment()],
                'Yesterday': [moment().subtract('days', 1), moment().subtract('days', 1)],
                'Last 7 Days': [moment().subtract('days', 6), moment()],
                'Last 30 Days': [moment().subtract('days', 29), moment()],
                'This Month': [moment().startOf('month'), moment().endOf('month')],
                'Last Month': [moment().subtract('month', 1).startOf('month'), moment().subtract('month', 1).endOf('month')]
            },
            buttonClasses: ['btn'],
            applyClass: 'green',
            cancelClass: 'default',
            format: 'MM/DD/YYYY',
            separator: ' to ',
            locale: {
                applyLabel: 'Apply',
                fromLabel: 'From',
                toLabel: 'To',
                customRangeLabel: 'Custom Range',
                daysOfWeek: ['Su', 'Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa'],
                monthNames: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'],
                firstDay: 1
            }
        },
            function (start, end) {
                console.log("Callback has been called!");
                $('#reportrange span').html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'));
            }
        );
        //Set the initial state of the picker label
        $(container).find('#reportrange span').html(moment().subtract('days', 29).format('MMMM D, YYYY') + ' - ' + moment().format('MMMM D, YYYY'));
    },

    handleDatetimePicker: function (container) {
        $(container).find(".form_meridian_datetime").datetimepicker({
            isRTL: HobbyClue.isRTL,
            format: "M d yyyy H:mm P",
            showMeridian: true,
            autoclose: true,
            pickerPosition: (HobbyClue.isRTL ? "bottom-right" : "bottom-left"),
            todayBtn: true
        });

        $('body').removeClass("modal-open"); // fix bug when inline picker is used in modal
    },
    
    handleScrollers: function (container) {
        $(container).find('.scroller').each(function () {
            var height;
            if ($(this).attr("data-height")) {
                height = $(this).attr("data-height");
            } else {
                height = $(this).css('height');
            }
            $(this).slimScroll({
                size: '7px',
                color: ($(this).attr("data-handle-color") ? $(this).attr("data-handle-color") : '#a1b2bd'),
                railColor: ($(this).attr("data-rail-color") ? $(this).attr("data-rail-color") : '#333'),
                position: 'right',
                height: height,
                alwaysVisible: ($(this).attr("data-always-visible") == "1" ? true : false),
                railVisible: ($(this).attr("data-rail-visible") == "1" ? true : false),
                disableFadeOut: true
            });
        });
    },

    handleInputMasks: function (container) {
        $.extend($.inputmask.defaults, {
            'autounmask': true
        });

        $(container).find(".mask_url").inputmask("http://", {
            autoUnmask: true
        }); //direct mask        
        $(container).find(".mask_url").inputmask("d/m/y", {
            "placeholder": "http://"
        }); //change the placeholder
    },

    handleBootstrapSelect: function (container) {
        $(container).find('.bs-select').selectpicker({
            iconBase: 'fa',
            tickIcon: 'fa-check'
        });
    },

    handleOverlays: function (container) {
        $(container).find(".fancybox").fancybox({
            padding: 0,
            fitToView: true, // avoids scaling the image to fit in the viewport
            beforeShow: function () {
                // set size to (fancybox) img
                $(".fancybox-image").css({
                    "width": 800,
                    "height": 600
                });
                // set size for parent container
                this.width = 800;
                this.height = 600;
            },
            afterLoad: function () {
                this.inner.prepend('<div style="position: absolute; top: 0; left: 0;"><h1>1. My custom title</h1></div>');
            }
        });
    },

    handleWysihtml5: function (container) {
        if (!jQuery().wysihtml5) {
            return;
        }

        if ($(container).find('.wysihtml5').size() > 0) {
            $(container).find('.wysihtml5').wysihtml5({
                "stylesheets": ["../../Content/plugins/bootstrap/bootstrap-wysihtml5/wysiwyg-color.css"],
                "font-styles": true, //Font styling, e.g. h1, h2, etc. Default true
                "emphasis": true, //Italics, bold, etc. Default true
                "lists": true, //(Un)ordered lists, e.g. Bullets, Numbers. Default true
                "html": false, //Button which allows you to edit the generated HTML. Default false
                "link": true, //Button to insert a link. Default true
                "image": false, //Button to insert an image. Default true,
                "color": false //Button to change color of font  
            });
        }
    },

    init: function (container) {
        if (!container)
            container = $('body');

        HobbyClue.handleBootstrapSelect(container);
        HobbyClue.handleTimePickers(container);
        HobbyClue.handleDatePickers(container);
        HobbyClue.handleDatetimePicker(container);
        HobbyClue.handleDateRangePickers(container);
        HobbyClue.handleInputMasks(container);
        HobbyClue.handleWysihtml5(container);
        HobbyClue.handleScrollers(container); // handles slim scrolling contents 
        jQuery("abbr.timeago").timeago();

        jQuery.extend(jQuery.fn, {
            // Name of our method & one argument (the parent selector)
            within: function (pSelector) {
                // Returns a subset of items using jQuery.filter
                return this.filter(function () {
                    // Return truthy/falsey based on presence in parent
                    return $(this).closest(pSelector).length;
                });
            }
        });

        //causes remote modals to reload themselves on click
        $('body').on('hidden.bs.modal', '.modal', function () {
            $(this).data('modal', null);
            $(this).removeData('bs.modal');
        });

    }
};

HobbyClue.Ajax = {
    ajaxJsonPost: function (antiforgeryToken, url, data, successFunction, errorFunction, method) {
        var httpMethod = method || "POST";
        $.ajax({
            beforeSend: function(request) {
                request.setRequestHeader("RequestVerificationToken", antiforgeryToken);
            },
            url: url,
            type: httpMethod,
            data: JSON.stringify(data),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function(item) {
                successFunction(item);
            },
            error: function(jqXHR, textStatus, errorThrown) {
                errorFunction(jqXHR, textStatus, errorThrown);
            }
        });
    }
};


HobbyClue.Header = {
    init: function() {
        $('.registerNewUser').click(function () {
            $("#newUserModal").modal('show');
            return false;
        });

        $('.lnkLogin').click(function () {
            $("#loginModal").modal('show');
            return false;
        });
    }
};

HobbyClue.EventModal = {
    constructEventObj: function () {
        var modal = $('.eventModal');

        var newEvent = {
            Id: $('#eventModalContent', modal).attr('data-id'),
            ParentId: $('#eventModalContent', modal).attr('data-parentid'),
            CreatedBy: $('#eventModalContent', modal).attr('data-createdby'),
            CreatedDate: $('#eventModalContent', modal).attr('data-createddate'),
            Name: $('#eventname', modal).val(),
            Description: $('#description', modal).val(),
            Location: $('#location', modal).val(),
            Images: $('#uploadbox').attr('data-image'),
            GroupId: $('#inner').attr('data-groupid'),
            MaxAttendees: $('#maxAttendees', modal).val(),
            StartDateTime: $('#startDate', modal).val() + ' ' + $('#StartTime', modal).val()
        };

        var repeatType = $('input:radio[name="eventrepeat"]:checked').attr('data-repeattype');
        switch (repeatType) {
            case 'norepeat':
                newEvent.OneTimeOnlyEventDate = $('#startDate', modal).val() + ' ' + $('#StartTime', modal).val();
                newEvent.Frequency = 0; //FrequencyTypeEnum
                break;
            case 'daily':
                newEvent.Frequency = 1;
                var daysOfWeekEnum = 0;
                $.each($("input:checked[type='checkbox'].dailyday"), function (index, chkbox) {
                    var chkBoxValue = $(this).val();
                    daysOfWeekEnum = daysOfWeekEnum | chkBoxValue;
                });
                newEvent.DaysOfWeek = daysOfWeekEnum;
                break;
            case 'weekly':
                newEvent.Frequency = 2;
                newEvent.DaysOfWeek = $('.bs-select.day').selectpicker('val'); //DaysOfWeekEnum
                break;
            case 'monthly':
                newEvent.Frequency = 4;
                newEvent.DaysOfWeek = $('.bs-select.monthday').selectpicker('val');
                newEvent.MonthlyInterval = $('.bs-select.weeknum').selectpicker('val'); //week num
                break;
            case 'specificday':
                newEvent.Frequency = 4;
                newEvent.DayOfMonth = $('#dayspinner', modal).find('input.spinner-input').val();;
                break;
        }

        if ($('#EndTime', modal).is(":visible")) {
            newEvent.EndDateTime = $('#startDate', modal).val() + ' ' + $('#EndTime', modal).val();
        }
        
        return newEvent;
    },

    wireElements: function(locationBox) {
        if (jQuery().timepicker) {
            //Match start time to end time
            $('.startTime').on('changeTime.timepicker', function (e) {
                if ($('.endTime:visible').length == 0) {
                    $('.endTime').timepicker('setTime', $(this).val());
                }
            });

            $('#uploadbox').singleupload({
                action: '/file/singlefileupload',
                inputId: 'singleupload_input',
                onError: function (code) {
                    console.debug('error code ' + res.code);
                },
                onSuccess: function (url, data) {
                    $('#uploadbox').attr('data-image', url);
                }
                /*,onProgress: function(loaded, total) {} */
            });
        }

        if (jQuery().datepicker) {
            $('#startDatePicker').on('changeDate', function (ev) {
                if ($('#endDatePicker:visible').length == 0) {
                    $('#endDatePicker').children('input').val($(this).children('input').val());
                }
            });
        }

        //Toggle end time
        $('body').on('click', '.showEndTime', function () {
            $(this).hide();
            $(this).parents('ul:first').find('.endTimeSection').show();
        });

        //Wire up day spinner
        $('#dayspinner').spinner({ value: 1, min: 1, max: 31 });

        //Repeat settings change event
        $('#repeatSettings input[name=eventrepeat]:radio').on('change', function () {
            $('#repeatBlocks > li').hide();
            $('#repeatBlocks > li#' + $(this).data('repeattype')).show();

            $('#repeatSettings input[name=eventrepeat]:radio').removeClass('active');
            $(this).addClass('active');
        });


        //Allow only numbers in attendees box
        $("#maxAttendees").inputmask({ "mask": "9", "repeat": 4, "greedy": false });

        //Wire up location service
        new google.maps.places.SearchBox(locationBox);
    },

    wireValidation: function (headerToken)
    {
        $("#newevent-form").validate({
            rules: {
                dailyday: {
                    required: true,
                    minlength: 1
                },
                eventname: {
                    required: true,
                    minlength: 4,
                    maxlength: 50
                },
                location: {
                    required: true,
                    minlength: 4,
                    maxlength: 1024
                },
                weeklyEndTime: { greaterThan: "#StartDate" }
            },
            messages: {
                eventname: {
                    required: "Please enter a name for your event"
                },
                location: {
                    required: "Please enter a location for your event"
                }
            },
            submitHandler: function (form) {
                var eventObj = HobbyClue.EventModal.constructEventObj();
                var mode = $('#eventModalContent').attr('data-mode');
                $.ajax({
                    beforeSend: function (request) {
                        request.setRequestHeader("RequestVerificationToken", headerToken);
                    },
                    url: '/api/events/' + mode,
                    type: 'POST',
                    data: JSON.stringify(eventObj),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (item) {
                        var jsonEventModel = ko.mapping.fromJS(item);
                        viewModel.addEventComputedColumns(jsonEventModel);
                        if (mode.toLowerCase() == 'create') {
                            viewModel.upcomingEvents.unshift(jsonEventModel);
                            //sort in descending order
                            viewModel.events.sort(function (l, r) {
                                return (Date.parse(l.startDate()) == Date.parse(r.startDate()) ? 0 :
                                       (Date.parse(l.startDate()) < Date.parse(r.startDate()) ? -1 : 1));
                            });
                            $("html, body").animate({ scrollTop: $("#event" + item.id).offset().top - 100 }, 1000);
                        }
                        else if (mode.toLowerCase() == 'edit') {
                            var match = ko.utils.arrayFirst(viewModel.upcomingEvents(), function (ev) {
                                return item.id === ev.id();
                            });
                            if (match) {
                                viewModel.upcomingEvents.replace(match, jsonEventModel);
                            }
                        }
                        $('.eventModal').modal('hide');
                    },
                    error: function (jqXHR, textStatus, errorThrown) { }
                });
            }
        });

    }

}

HobbyClue.NewActivity =
{
    ValidateActivityForm: function () {
        var newActivityFieldContainer = $('#newActivityFieldContainer');
        $(newActivityFieldContainer).wrap('<form action="/" id="validationform" method="post" novalidate="novalidate" />');
        var submitForm = $('#validationform');
        
        $(submitForm).validate({
            ignore: [],
            rules: {
                Description: { required: true },
                Categories: { required: true },
                newActivityLocation: { required: true }
            },
            messages: {
                Description: "Please enter a description",
                Categories: "Please enter at least 1 category",
                newActivityLocation: "Please select a location"
            },
            submitHandler: function (form) {
                form.submit();
            }
        });
        var isValid = $(submitForm).valid();

        var hasImageErrorBox = $('#customeimageerror', '#newActivityBox');
        var hasImage = $('#uploadedImage', '#newActivityBox').attr('data-uploaded') == 'true';
        $(hasImageErrorBox).toggle(!hasImage);
        isValid = isValid && hasImage;
        
        $(newActivityFieldContainer).unwrap();
        return isValid;
    },

    _getYoutubeVideoId: function(webUrl) {
        var videoId = webUrl.split('v=')[1];
        var ampersandPosition = videoId.indexOf('&');
        if (ampersandPosition != -1) {
            videoId = videoId.substring(0, ampersandPosition);
        }
        return videoId;
    },

    _wireLocationBox: function (locationbox) {
        var aclocation = new google.maps.places.Autocomplete(locationbox);
        HobbyClue.Maps.wireGenerateAddress(aclocation, locationbox);
    },
};


HobbyClue.Maps =
{
   initialize: function(mapcanvas, input, coordinates, zoom) {
        var mapOptions = {
            center: new google.maps.LatLng(coordinates.latitude, coordinates.longitude),
            zoom: zoom
        };
        var map = new google.maps.Map(mapcanvas, mapOptions);
        map.controls[google.maps.ControlPosition.TOP_LEFT].push(input);

        var autocomplete = new google.maps.places.Autocomplete(input);
        //autocomplete.bindTo('bounds', map);

        var infowindow = new google.maps.InfoWindow();
        var marker = new google.maps.Marker({ map: map });

        google.maps.event.addListener(autocomplete, 'place_changed', function() {
            infowindow.close();
            marker.setVisible(false);
            var place = autocomplete.getPlace();
            $(mapcanvas).closest('.hdnActivityLocation').val(JSON.stringify(place));
            if (!place.geometry) {
                return;
            }

            // If the place has a geometry, then present it on a map.
            if (place.geometry.viewport) {
                map.fitBounds(place.geometry.viewport);
            } else {
                map.setCenter(place.geometry.location);
                map.setZoom(17); // Why 17? Because it looks good.
            }
            marker.setIcon({
                url: place.icon,
                size: new google.maps.Size(71, 71),
                origin: new google.maps.Point(0, 0),
                anchor: new google.maps.Point(17, 34),
                scaledSize: new google.maps.Size(35, 35)
            });
            marker.setPosition(place.geometry.location);
            marker.setVisible(true);

            var address = '';
            if (place.address_components) {
                address = [
                    (place.address_components[0] && place.address_components[0].short_name || ''),
                    (place.address_components[1] && place.address_components[1].short_name || ''),
                    (place.address_components[2] && place.address_components[2].short_name || '')
                ].join(' ');
            }

            infowindow.setContent('<div><strong>' + place.name + '</strong><br>' + address);
            infowindow.open(map, marker);
        });
    },

   wireGoogleMapVenueFinder: function (callback) {
        var options = {
            enableHighAccuracy: true,
            timeout: 2000,
            maximumAge: 1000
        };

        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(callback, HobbyClue.Maps._locationError, options);
        }
    },
    
    _locationError: function(position) {
        alert("Error getting location");
    },

    wireGenerateAddress: function (autocompleteBox, htmlBox) {
        google.maps.event.addListener(autocompleteBox, 'place_changed', function() {
            var place = autocompleteBox.getPlace();
            var hiddenAddressId = $(htmlBox).attr('id') + '_address';
            if ($('#' + hiddenAddressId).length == 0) {
                $(htmlBox).parent().append("<input type='hidden' id='" + hiddenAddressId + "' />");
            }
            $('#' + hiddenAddressId).val(JSON.stringify(place));
        });
    }
};


HobbyClue.Timeline = {
    wire: function(viewModel) {
        $(window).scroll(function () {
            if ($(window).scrollTop() == $(document).height() - $(window).height()) {
                HobbyClue.Timeline.loadMoreClues(viewModel);
            }
        });

        $('.closeShare').on('click', function () {
            $(this).parents('.container:first').find('.action-box.main').show();
            $(this).parents('.container:first').find('.action-box.social').hide();
        });

        $('.share', '.action-box.main').on('click', function () {
            $(this).parents('.container:first').find('.action-box.main').hide();
            $(this).parents('.container:first').find('.action-box.social').show();
        });

    },
    
    loadMoreClues: function (viewModel) {
        var selectedTags = [];
        //if (viewModel.sidebarItems.selectedTag.id() > 0)
        //    selectedTags.push(viewModel.sidebarItems.selectedTag.name());

        var params = { Page: viewModel.page() + 1, Tags: selectedTags, CityName: $('#cityText').data('cityname'), Region: $('#cityText').data('region') };
        $.ajax({
            url: '/api/card?' + jQuery.param(params),
            type: 'GET',
            data: {},
            success: function(data) {
                if (data != 'undefined' && data.length > 0) {
                    viewModel.page(viewModel.page() + 1);
                    $.each(data, function (i, item) {
                        item.createdDate = HobbyClue.General.formatDate(new Date(item.createdDate));
                        var jsonModel = ko.mapping.fromJS(item);
                        viewModel.appendActivity(jsonModel);
                    });
                }
            },
            error: function(jqXHR, textStatus, errorThrown) {

            },
        });
    },

    initialize: function (jsonString, currentUserId) {
        var viewModel = ko.mapping.fromJS(jsonString); 
        viewModel.searchIcon = 'icon-search';
        viewModel.templateName = function (activity) { return 'Image-template'; };
        viewModel.incrementVote = function () {
            changeVote(this.id(), this.votes, currentUserId, 1);
        };
        viewModel.decrementVote = function () {
            changeVote(this.id(), this.votes, currentUserId, 0);
        };
        viewModel.openPopup = function(idx) {
            $('.pic .entry-gallery').magnificPopup('open', idx);
        };
        viewModel.page = ko.observable(1),
        viewModel.alltags = ko.observableArray([]);
        viewModel.cardTags = ko.observableArray([]);
        viewModel.appendActivity = function (activity) {
            addComputedColumns(activity);
            viewModel.timeline.activities.push(activity);
        };
        viewModel.prependActivity = function (activity) {
            addComputedColumns(activity);
            viewModel.timeline.activities.unshift(activity);
        };
     
        viewModel.removeActivity = function (activity, event) {
            $.ajax({
                type: 'DELETE',
                url: "/api/card/" + activity.id(),
                dataType: 'text'
            }).done(function (data) {
                viewModel.timeline.activities.remove(activity);
            });
        };
        $.each(viewModel.timeline.activities(), function(index, activity) {
            addComputedColumns(activity);
            getFbCommentcount(activity);
        });
        

        viewModel.AddNewImage = function() {
            $('#lnkshowuploadimageform').click();
            $("#addModal").modal('show');
            viewModel.LoadNewClueForm();
            return false;
        };

        viewModel.AddCardTags = function () {
            if (HobbyClue.NewActivity.ValidateActivityForm()) {
                viewModel.loadAllTags();
                $("#addModal").modal('hide');
                $("#selectcardtags").modal('show');
            }
        };

        viewModel.LoadNewClueForm = function() {
            $('#newActivityFieldContainer').empty();
            var url = "/cards/newimageupload";
            $.get(url, function(data) {
                $("#newActivityBox").html(data);
                viewModel.cardTags([]);
            });
        };
        
        viewModel.SaveActivity = function () {
            var tagselected = $('.cardtag.selected, #cardtags').length > 0;
            $('#cardtagserror').toggle(!tagselected);

            if (tagselected) {
                var newActivityBox = $('#newActivityBox');
                var newActivity = {
                    city: viewModel.cityId(),
                    Description: $(newActivityBox).find('#Description').val(),
                    ImageUrl: $(newActivityBox).find('#uploadedImage').attr('src'),
                    ThumbnailUrl: $(newActivityBox).find('.activityImage:first > img').attr('src'),
                    Location: $(newActivityBox).find('#newActivityLocation_address').val(),
                    Type: 'Image',
                    Tags: ko.toJS(viewModel.cardTags)
                };

                $.ajax({
                    type: 'Put',
                    url: "/api/card",
                    data: JSON.stringify(newActivity),
                    dataType: 'json',
                    contentType: "application/json; charset=utf-8",
                    success: function (item) {
                        item.createdDate = HobbyClue.General.formatDate(new Date(item.createdDate));
                        var jsonModel = ko.mapping.fromJS(item);
                        viewModel.prependActivity(jsonModel);
                        $("#selectcardtags").modal('hide');
                    }
                });
            }
        },
        
        viewModel.userTagSelectModal = function () {
            viewModel.loadAllTags();
            $("#tagSelectModal").modal('show');
        },

        viewModel.loadAllTags = function () {
            $.get('/api/tag').done(function (data) {
                viewModel.alltags([]);
                $.each(data, function (i, item) {
                    var tagModel = ko.mapping.fromJS(item);
                    tagModel.isCardTag = ko.computed(function () {
                        return $.grep(viewModel.cardTags(), function (n) { return n.id() == tagModel.id(); }).length > 0;
                    });
                    viewModel.alltags.push(tagModel);
                });
            });
            return false;
        };

        viewModel.togglecardtagselect = function () {
            var newTag = this;
            if (!newTag.isCardTag()) {
                viewModel.cardTags.push(newTag);
            } else
                viewModel.cardTags.remove(function (item) {
                    return item.id() == newTag.id();
                });
        };
        ko.applyBindings(viewModel);
        
        HobbyClue.Timeline.wire(viewModel);
        HobbyClue.Timeline.wireImagePopups();
        
        function addComputedColumns(activity) {
            activity.thumbsUpCount = ko.computed(function () {
                return findVotes(activity.votes(), 1);
            });
            activity.thumbsDownCount = ko.computed(function () {
                return findVotes(activity.votes(), 0);
            });
            activity.commentCount = ko.observable(0);
        }

        function getFbCommentcount(activity) {
            var url = 'http://hobbyclue.com/' + activity.id();
            $.getJSON('http://graph.facebook.com/comments?id=' + url, function (response) {
                if (!response['error']) {
                    activity.commentCount(response.data.length);
                }
            });
        };

        function findVotes(votes, voteType) {
            return $.grep(votes, function (e) { return e.vote() == voteType; }).length;
        }

        function changeVote(cardId, votesObservableArray, userId, value) {
            var currentuservote = $.grep(votesObservableArray(), function(e) {
                 return e.userId() == userId;
            });
            if (currentuservote.length > 0) {
                if (currentuservote.value == value) return;
                currentuservote[0].vote(value);
            } else {
                votesObservableArray.push({ userid: userId, vote: ko.observable(value) });
            }
            HobbyClue.Votes.Save(value, cardId);
        }
    },

    wireImagePopups: function () {
        $('.pic .entry-gallery').magnificPopup({
            delegate: 'a.caption-link', // child items selector, by clicking on it popup will open
            type: 'image',
            image: {
                titleSrc: 'title',
                markup: '<div class="fullcard">' +
                            '<div class="row" style="margin-right: 2%;"> ' +
                                '<div class="mfp-close clearfix"></div>' +
                            '</div>' +
                            '<div class="row"> ' +
                            '   <div class="details col-md-7 col-md-offset-1" style="width: auto; max-width: 60%;"> ' +
                            '       <div class="mfp-img"></div>' +
                                  '       <div class="user-profile-pic text-left">' +
                            '           <img class="authorImg" data-bind="attr: {src: creator.avatar}" alt="">' +
                            '       </div>' +
                            '       <div class="authorUsername"></div>' +
                            '       <div class="img-desc" style="background-color: #FFF; padding: 10px; min-height: 200px"></div>' +
                            '   </div>' +
                            '   <div class="col-md-5 fb-comments-container" style="width: 550px;"> ' +
                            '       <h2>Comments</h2>' +
                            '       <div class="fb-comments" data-href="http://hobbyclue.com/%id%" data-width="500" data-numposts="5" data-colorscheme="light"></div>' +
                            '   </div>' +
                            '</div>' +
                        '</div>'
            },

            gallery: {
                enabled: true,
                navigateByImgClick: true,
            },
            removalDelay: 300,
            mainClass: 'mfp-fade',

            callbacks: {
                open: function (data) {
                    FB.XFBML.parse();
                },
                close: function () {

                },
                markupParse: function (template, values, item) {
                    template.find('.fb-comments').attr('data-href', 'http://hobbyclue.com/' + $(item.el.context).parents('.picturecard:first').attr('data-activityid'));

                    $(item.el.context).siblings('.content-timebox').children('abbr').clone().appendTo(template.find('.details'));
                    template.find('.img-desc').html($(item.el.context).parents('.entry-thumb').attr('data-desc'));
                    template.find('.authorUsername').html($(item.el.context).parents('.entry-thumb').attr('data-author'));
                    template.find('.authorUsername').html($(item.el.context).parents('.entry-thumb').attr('data-author'));
                    template.find('.authorImg').attr('src', $(item.el.context).parents('.entry-thumb').attr('data-avatar'));
                }
            }
        });
    },

    initializeEndlessScroll: function (successFunction) {
        $(window).scroll(function() {
            if ($(window).scrollTop() == $(document).height() - $(window).height()) {
                successFunction();
            }
        });
    },

    getTimeAgo: function(varDate) {
        return varDate ? $.timeago(varDate.toString().slice(-1) == 'Z' ? varDate : varDate + 'Z') : '';
    }
};

HobbyClue.KnockoutExtensions = {
    initialize: function () {
        ko.bindingHandlers.initializeTabs = {
            update: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
                // First get the latest data that we're bound to
                var value = valueAccessor();

                // Next, whether or not the supplied model property is observable, get its current value
                var valueUnwrapped = ko.unwrap(value);

                $(element).tabs({ active: valueUnwrapped });
            }
        };

        ko.bindingHandlers.initializeGallery = {
            init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
                // First get the latest data that we're bound to
                var value = valueAccessor();

                // Next, whether or not the supplied model property is observable, get its current value
                var valueUnwrapped = ko.unwrap(value);

                $(element).magnificPopup({
                    delegate: 'a', // child items selector, by clicking on it popup will open
                    type: 'image',
                    image: { titleSrc: 'title' },
                    gallery: {
                        enabled: true,
                        navigateByImgClick: true,
                    },
                    // Animation
                    removalDelay: valueUnwrapped,
                    mainClass: 'mfp-fade'
                });
            }
        };

        ko.bindingHandlers.initializeSlimScroll = {
            update: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
                var heightValue = ko.unwrap(valueAccessor());
                if (heightValue != -1) {
                    $(element).slimscroll({
                        height: heightValue + 'px',
                        railVisible: true,
                        alwaysVisible: true
                    });
                }
            }
        };

        //textarea autosize
        ko.bindingHandlers.jqAutoresize = {
            init: function (element, valueAccessor, aBA, vm) {
                $(element).autosize();
            }
        };
        
        ko.bindingHandlers.timeAgo = {
            update: function (element, valueAccessor, aBA, vm) {
                var varDate = ko.unwrap(valueAccessor());
                $(element).text(varDate ? $.timeago(varDate.toString().slice(-1) == 'Z' ? varDate : varDate + 'Z') : '');
            }
        }
    }
};

HobbyClue.General = {
    formatDate: function(date) {
        var m_names = new Array("Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec");
        var curr_date = date.getDate();
        var curr_month = date.getMonth();
        var curr_year = date.getFullYear();
        return m_names[curr_month] + " " + curr_date + " " + curr_year;
    },

    loadFbSdk: function(appId){
        window.fbAsyncInit = function () {
            FB.init({
                appId: appId,
                status     : true, // check login status
                cookie     : true, // enable cookies to allow the server to access the session
                xfbml      : true,  // parse XFBML
                version: 'v2.0'
            });
        };

        (function (d, s, id) {
            var js, fjs = d.getElementsByTagName(s)[0];
            if (d.getElementById(id)) { return; }
            js = d.createElement(s); js.id = id;
            js.src = "//connect.facebook.net/en_US/sdk.js";
            fjs.parentNode.insertBefore(js, fjs);
        }(document, 'script', 'facebook-jssdk'));
    },

    guid: function () {
        var s4 = (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
        return (s4 + s4 + "-" + s4 + "-" + s4 + "-" + s4 + "-" + s4 + s4 + s4);
    },

    getNumericSuffix: function(num) {
        if (num == 1 || num == 21 || num == 31) {
            return "st";
        } else if (num == 2 || num == 22) {
            return "nd";
        } else if (num == 3) {
            return "rd";
        }
        return "th";
    }
    
};

HobbyClue.MultiImageUpload = {

    wireUploadBtn: function(uploadBtn) {
       $(uploadBtn).addClass('btn btn-primary')
                   .prop('disabled', true)
                   .text('Processing...')
                   .on('click', function () {
                        var $this = $(this);
                        $this.off('click').text('Abort').on('click', function () {
                            $this.remove();
                            $this.data().abort();
                        });
                        $this.data().submit().always(function () { $this.remove(); });
                   });
    },

    wire: function(uploadForm, successFnctn) {
        $(uploadForm).fileupload({
            url: '/file/ajaxupload',
            paramName: "files",
            formData: { tempid: HobbyClue.General.guid() },
            autoUpload: true,
            acceptFileTypes: /(\.|\/)(gif|jpe?g|png)$/i,
            maxFileSize: 5000000, // 5 MB
            // Enable image resizing, except for Android and Opera,
            // which actually support image resizing, but fail to
            // send Blob objects via XHR requests:
            disableImageResize: /Android(?!.*Chrome)|Opera/
                .test(window.navigator.userAgent),
            previewMaxWidth: 200,
            previewMaxHeight: 150,
            previewCrop: true
        })
        .on('fileuploadadd', function (e, data) { })
        .on('fileuploadprocessalways', function (e, data) { })
        .on('fileuploadfail', function (e, data) {
            $.each(data.files, function (index) {
                var error = $('<span class="text-danger"/>').text('File upload failed.');
                $(data.context.children()[index]).append('<br>').append(error);
            });
        })
        .on('fileuploaddone', function (e, data) { successFnctn(e, data); })
        .prop('disabled', !$.support.fileInput)
        .parent().addClass($.support.fileInput ? undefined : 'disabled');

        $('.files').on('click', 'a.delete', function () {
            $(this).parents('li:first').remove();
        });
    },

    addPicPreviewToEventList: function (imageUrl, imageHtml, picList) {
        var previewElement = $('<li/>').appendTo(picList);
        if (!imageHtml || imageHtml === undefined) {
            imageHtml = $('<img/>').attr({'src': imageUrl,'width': 200, 'height': 150});
        }
        $(previewElement).attr('data-filename', imageUrl).prepend('<a href="#" class="delete"><i class="fa fa-trash-o"></i></a>').prepend(imageHtml);
    }
};

HobbyClue.Votes = {
    Save: function(voteType, cardId) {
        var params = { Vote: voteType, CardId: cardId };
        $.ajax({
            url: '/api/votes',
            type: 'POST',
            data: params,
            success: function(data) {},
            error: function(jqXHR, textStatus, errorThrown) {},
        });
    }
};

HobbyClue.Sidebar = {
    wire: function () {
        //HobbyClue.ScrollDiv.wire(content);

        //$(window).resize(function () {
        //    HobbyClue.ScrollDiv.resizeScrollDiv(content);
        //});

        $('#sidebar a.header').on('click', function () {
            var tagsSection = $(this).parents('ul.tags:first');
            if (!$(tagsSection).hasClass('selected')) {
                $('ul.tags').removeClass('selected', 200);
                $(tagsSection).addClass('selected', 200);
            }
            return false;
        });
    }
};


HobbyClue.User =
{
    ValidateRegisterUserForm: function () {
        var submitForm = $('form#newUserForm');

        $(submitForm).validate({
            ignore: [],
            rules: {
                UserName: { required: true },
                Password: { required: true },
                ConfirmPassword: { equalTo: "#Password" },
                Email: {
                    required: true,
                    email: true,
                    remote: {
                        url: "/api/users/EmailNotExists",
                        type: "post"
                    }
                }
            },
            messages: {
                UserName: "Please enter a username",
                Email: {
                    required: "Please enter an email",
                    email: "This is not a valid email!",
                    remote: "Email already in use!"
                },
                Password: "Please enter a password",
                ConfirmPassword: "Passwords do not match"
            },
            submitHandler: function (form) {
                form.submit();
            }
        });
        return $(submitForm).valid();
    },

    ValidateSignInForm: function () {
        var submitForm = $('form#loginForm');

        $(submitForm).validate({
            ignore: [],
            rules: {
                Password: { required: true },
                Email: {
                    required: true,
                    email: true
                }
            },
            messages: {
                Email: {
                    required: "Please enter an email",
                    email: "This is not a valid email!",
                    remote: "Email already in use!"
                },
                Password: "Please enter a password"
            },
            submitHandler: function (form) {
                form.submit();
            }
        });
        return $(submitForm).valid();
    },
};

HobbyClue.ScrollDiv = {
    wire: function(scrolldiv) {
        $(scrolldiv).slimScroll({
            disableFadeOut: true,
            color: '#f89406',
            distance: '5px',
            opacity: 1,
            height: HobbyClue.ScrollDiv.userTagsHeight(scrolldiv)
        });
    },

    resizeScrollDiv: function (scrolldiv) {
        var newheight = HobbyClue.ScrollDiv.userTagsHeight(scrolldiv);
        $(scrolldiv).css("height", newheight + 'px');
        $(scrolldiv).parents('.slimScrollDiv:first').css("height", newheight + 'px');
    },

    userTagsHeight: function(innercontent) {
        return $(window).height() - $(innercontent).offset().top - 50;
    }
};


HobbyClue.Location = {

    SetCurrentCity: function() {
        var options = {
            enableHighAccuracy: true,
            timeout: 2000,
            maximumAge: 1000
        };

        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(HobbyClue.Location.City, HobbyClue.Location.Error, options);
        }
    },


    City: function(position) {
        var coordinates = position.coords;

        $.ajax({
            url: "http://maps.googleapis.com/maps/api/geocode/json?latlng=" + coordinates.latitude + "," + coordinates.longitude + "&sensor=false",
            dataType: 'json',
            beforeSend: function(xhr) {
                xhr.overrideMimeType("application/json; charset=x-user-defined");
            }
        }).done(function(data) {
            var addresscomponents = data.results[4].address_components;
            var city_name = addresscomponents[1].long_name;
            var region = addresscomponents[3].long_name;
            var country = addresscomponents[4].long_name;
            $.post('/api/users/setCity/', { name: city_name, region: region, country: country })
                .done(function(data) {
                    if (Modernizr.localstorage) {
                        // run localStorage code here... 
                        localStorage.setItem("usercurrentcity", city_name + "," + region + "," + country);
                    } else {
                        // no native support for local storage -- use cookies instead
                    }
                });

        });
        
    },

    Error: function(position) {
        switch (position.code) {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            default:
                break;
        }
    },

};

HobbyClue.Markdown = {
    initializeNewMessageEditor: function (rootNode, afterShow, saveFunction, afterHideFunction, hidetitleBox, autofocus, saveBtnText) {
        autofocus = typeof autofocus !== 'undefined' ? autofocus : false;
        saveBtnText = typeof saveBtnText !== 'undefined' ? saveBtnText : 'Post';
        rootNode.find(".open-editor").click(function () {
            var askSection = rootNode.find(".chat-form");
            var askSectionContainer = askSection.parent();
            var questionTitleBox = $('.newTitle', askSectionContainer);
            askSection.siblings("textarea.mdeditor").show().markdown({
                autofocus: autofocus,
                savable: true,
                hideable: false,
                height: 'span2',
                onShow: function (e) {
                    HobbyClue.Markdown.updateEditorControls(e, saveBtnText);
                    $('.open-editor', rootNode).unbind('click');
                    rootNode.on('click', '.open-editor', function () {
                        HobbyClue.Markdown.showEditor(askSectionContainer, e, hidetitleBox, questionTitleBox, afterHideFunction);
                        if (autofocus) {
                            e.$textarea.focus();
                            e.$editor.addClass('active');
                        }
                        afterShow(this, e);
                    });
                    HobbyClue.Markdown.showEditor(askSectionContainer, e, hidetitleBox, questionTitleBox, afterHideFunction);
                    afterShow(this, e);
                },
                onSave: function (e) {
                    saveFunction(e);
                    askSectionContainer.find('.closeEditor').click();
                }
            });
        });
    },

    updateEditorControls: function (editorEvent, saveBtnText) {
        var editorControls = $('.md-controls', editorEvent.$editor);
        $(editorControls).empty();
        var closeElement = $('<a/>').appendTo(editorControls);
        $(closeElement).attr('href', 'javascript:;').append('<span class="closeEditor"> Close </span>');
        $('.md-footer > .btn.btn-success', editorEvent.$editor).text(saveBtnText);
    },

    hideEditorAndClearInputBoxes: function (container, e, titleboxHidden, titleBox) {
        e.$editor.hide();
        $.each(container.find('input[type="text"]'), function (index, inputBx) {
            $(inputBx).val('');
        });
        e.$textarea.val('');
        e.$editor.removeClass('active');

        if (titleboxHidden) {
            titleBox.show();
        }
    },

    showEditor: function (askSectionContainer, editorEvent, hidetitleBox, questionTitleBox, afterHideFunction) {
        if (hidetitleBox) {
            questionTitleBox.hide();
        }

        $('.closeEditor').unbind('click').on('click', function () {
            HobbyClue.Markdown.hideEditorAndClearInputBoxes(askSectionContainer, editorEvent, hidetitleBox, questionTitleBox);
            afterHideFunction(editorEvent);
        });

        editorEvent.$editor.show();
        editorEvent.$editor.addClass('active');
        HobbyClue.Markdown.enableCloseOnClickOutsideNewQuestion(questionTitleBox, editorEvent, askSectionContainer);
    },

    enableCloseOnClickOutsideNewQuestion: function (questionTitleBox, editorEvent, container) {
        $('html').click(function (e) {
            if ($.trim(questionTitleBox.val()) == '' && $.trim(editorEvent.getContent()) == '' && $(e.target).closest(container).length == 0 && !$(e.target).hasClass('mdeditorcntrl')) {
                $('.closeEditor', container).click();
                $('html').unbind('click');
            }
        });
    }
}

HobbyClue.Share = {
    // Social share
    share_winWidth: '520',
    share_winHeight: '350',

    share_winTop: (screen.height / 2) - (350 / 2),
    share_winLeft: (screen.width / 2) - (520 / 2),

    pukkaFBShare: function(url, title, descr, image) {
        window.open('http://www.facebook.com/sharer.php?m2w&s=100&p[title]=' + title + '&p[summary]=' + descr + '&p[url]=' + encodeURIComponent(url) + '&p[images][0]=' + image, 'sharer', 'top=' + HobbyClue.Share.share_winTop + ',left=' + HobbyClue.Share.share_winLeft + ',toolbar=0,status=0,width=' + HobbyClue.Share.share_winWidth + ',height=' + HobbyClue.Share.share_winHeight);
    },


    pukkaTWShare: function(url, title, descr, image) {
        window.open('https://twitter.com/share?url=' + url + '&text=' + title, 'sharer', 'top=' + HobbyClue.Share.share_winTop + ',left=' + HobbyClue.Share.share_winLeft + ',toolbar=0,status=0,width=' + HobbyClue.Share.share_winWidth + ',height=' + HobbyClue.Share.share_winHeight);
    },

    pukkaGPShare: function(url, title, descr, image) {
        window.open('https://plus.google.com/share?url=' + encodeURIComponent(url), 'sharer', 'top=' + HobbyClue.Share.share_winTop + ',left=' + HobbyClue.Share.share_winLeft + ',toolbar=0,status=0,width=' + HobbyClue.Share.share_winWidth + ',height=' + HobbyClue.Share.share_winHeight);
    },

    pukkaINShare: function(url, title, descr, image) {
        window.open('http://www.linkedin.com/shareArticle?mini=true&url=' + encodeURIComponent(url) + "&title=" + title + "&sumary=" + descr, 'sharer', 'top=' + HobbyClue.Share.share_winTop + ',left=' + HobbyClue.Share.share_winLeft + ',toolbar=0,status=0,width=' + HobbyClue.Share.share_winWidth + ',height=' + HobbyClue.Share.share_winHeight);
    },

    pukkaPTShare: function(url, title, descr, image) {
        window.open('http://pinterest.com/pin/create/button/?url=' + url + '&description=' + descr + '&media=' + image, 'sharer', 'top=' + HobbyClue.Share.share_winTop + ',left=' + HobbyClue.Share.share_winLeft + ',toolbar=0,status=0,width=' + HobbyClue.Share.share_winWidth + ',height=' + HobbyClue.Share.share_winHeight);
    }
	
}

HobbyClue.NewIntro = {
    wireIntroValidation: function() {
        $("#newIntro-form").validate({
            rules: {
                title: {
                    required: true,
                    minlength: 4,
                    maxlength: 50
                }
            },
            messages: {
                title: {
                    required: "Please enter a title for your intro"
                }
            },
            submitHandler: function (form) {
                var newIntro = {
                    Title: $('#title', form).val(),
                    Description: $('#description', form).val(),
                    VideoUrl: $('#videourl', form).val(),
                    Images: $('ul#newPicList > li', form).map(function () { return $(this).attr("data-filename"); }).get().join(",")
                };
                $.ajax({
                    beforeSend: function (request) {
                        request.setRequestHeader("RequestVerificationToken", $('#antiforgeryToken').val());
                    },
                    url: '/api/intros/create',
                    type: 'POST',
                    data: JSON.stringify(newIntro),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (item) {
                        //clear the modal
                        HobbyClue.NewIntro.clearForm(form);

                        //hide the modal
                        $('#newIntroModal').modal('hide');
                    },
                    error: function (jqXHR, textStatus, errorThrown) { }
                });
            }
        });
    },

    clearForm: function(form) {
        $('#title', form).val('');
        $('#description', form).val('');
        $('#videourl', form).val('');
        $('ul#newPicList > li', form).remove();
    },

    wireNewIntroImageUpload: function () {
    // Change this to the location of your server-side upload handler:
        HobbyClue.MultiImageUpload.wireUploadBtn($('<button/>'));
        var fileUploadInputBtn = $('#newintrofu');
        HobbyClue.MultiImageUpload.wire(fileUploadInputBtn, function (e, data) {
            var previewElement = $('<li/>').appendTo('ul#newPicList');
            $(previewElement).attr('data-filename', data.result.files[0]).prepend('<a href="#" class="delete"><i class="fa fa-trash-o"></i></a>').prepend(data.files[0].preview);
        });
    }
}

Number.prototype.padLeft = function (base, chr) {
    var len = (String(base || 10).length - String(this).length) + 1;
    return len > 0 ? new Array(len).join(chr || '0') + this : this;
}

Date.prototype.offset = function (offset) {
    // convert to msec
    // add local time zone offset
    // get UTC time in msec
    var utc = this.getTime() + (this.getTimezoneOffset() * 60000);

    // create new Date object for different city
    // using supplied offset
    return new Date(utc + (3600000 * offset));
}