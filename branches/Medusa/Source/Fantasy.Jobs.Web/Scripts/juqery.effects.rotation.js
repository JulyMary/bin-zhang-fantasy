(function ($)
{

    $.fn.rotate = function (options)
    {
        var settings =
        {
            axisX: 0,
            axisY: 0,
            radiusStart: 100,
            radiusEnd: 100,
            angleStart: 0,
            angleEnd: 360,
            duration: 'slow'
        };
        if (options)
        {
            $.extend(settings, options);
        }

        if (settings.duration == 'slow')
        {
            settings.duration = 600;

        }
        if (settings.duration == 'fast')
        {
            settings.duration = 200;

        }


        this.css("position", "absolute");

        var startTime = (new Date()).getTime();

        var animate = function (next)
        {
            var now = (new Date()).getTime();
            var lapse = now - startTime;
            if (lapse > settings.duration)
            {
                lapse = settings.duration;
            }

            var pos = lapse / settings.duration;
            var radius = (settings.radiusEnd - settings.radiusStart) * pos + settings.radiusStart;
            var a = (settings.angleEnd - settings.angleStart) * pos + settings.angleStart;
            a = a * Math.PI / 180;
            var x = Math.cos(a) * radius + settings.axisX;
            var y = Math.sin(a) * radius + settings.axisY;
            x = Math.round(x);
            y = Math.round(y);
            $(this).css("left", x);
            $(this).css("top", y);

            next();
            if (lapse < settings.duration)
            {
                $(this).delay(1).queue(animate);
            }


        }

        this.each(function ()
        {
            $(this).queue(animate);
        })


        return this;

    };
})(jQuery);
