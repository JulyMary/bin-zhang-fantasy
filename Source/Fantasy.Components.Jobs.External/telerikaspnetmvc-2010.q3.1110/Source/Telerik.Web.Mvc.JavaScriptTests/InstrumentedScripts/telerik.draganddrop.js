/* automatically generated by JSCoverage - do not edit */
try {
  if (typeof top === 'object' && top !== null && typeof top.opener === 'object' && top.opener !== null) {
    // this is a browser window that was opened from another window

    if (! top.opener._$jscoverage) {
      top.opener._$jscoverage = {};
    }
  }
}
catch (e) {}

try {
  if (typeof top === 'object' && top !== null) {
    // this is a browser window

    try {
      if (typeof top.opener === 'object' && top.opener !== null && top.opener._$jscoverage) {
        top._$jscoverage = top.opener._$jscoverage;
      }
    }
    catch (e) {}

    if (! top._$jscoverage) {
      top._$jscoverage = {};
    }
  }
}
catch (e) {}

try {
  if (typeof top === 'object' && top !== null && top._$jscoverage) {
    this._$jscoverage = top._$jscoverage;
  }
}
catch (e) {}
if (! this._$jscoverage) {
  this._$jscoverage = {};
}
if (! _$jscoverage['telerik.draganddrop.js']) {
  _$jscoverage['telerik.draganddrop.js'] = [];
  _$jscoverage['telerik.draganddrop.js'][1] = 0;
  _$jscoverage['telerik.draganddrop.js'][2] = 0;
  _$jscoverage['telerik.draganddrop.js'][23] = 0;
  _$jscoverage['telerik.draganddrop.js'][27] = 0;
  _$jscoverage['telerik.draganddrop.js'][28] = 0;
  _$jscoverage['telerik.draganddrop.js'][29] = 0;
  _$jscoverage['telerik.draganddrop.js'][34] = 0;
  _$jscoverage['telerik.draganddrop.js'][36] = 0;
  _$jscoverage['telerik.draganddrop.js'][39] = 0;
  _$jscoverage['telerik.draganddrop.js'][42] = 0;
  _$jscoverage['telerik.draganddrop.js'][43] = 0;
  _$jscoverage['telerik.draganddrop.js'][44] = 0;
  _$jscoverage['telerik.draganddrop.js'][48] = 0;
  _$jscoverage['telerik.draganddrop.js'][49] = 0;
  _$jscoverage['telerik.draganddrop.js'][50] = 0;
  _$jscoverage['telerik.draganddrop.js'][54] = 0;
  _$jscoverage['telerik.draganddrop.js'][55] = 0;
  _$jscoverage['telerik.draganddrop.js'][61] = 0;
  _$jscoverage['telerik.draganddrop.js'][62] = 0;
  _$jscoverage['telerik.draganddrop.js'][67] = 0;
  _$jscoverage['telerik.draganddrop.js'][68] = 0;
  _$jscoverage['telerik.draganddrop.js'][70] = 0;
  _$jscoverage['telerik.draganddrop.js'][73] = 0;
  _$jscoverage['telerik.draganddrop.js'][74] = 0;
  _$jscoverage['telerik.draganddrop.js'][75] = 0;
  _$jscoverage['telerik.draganddrop.js'][76] = 0;
  _$jscoverage['telerik.draganddrop.js'][79] = 0;
  _$jscoverage['telerik.draganddrop.js'][81] = 0;
  _$jscoverage['telerik.draganddrop.js'][82] = 0;
  _$jscoverage['telerik.draganddrop.js'][83] = 0;
  _$jscoverage['telerik.draganddrop.js'][87] = 0;
  _$jscoverage['telerik.draganddrop.js'][88] = 0;
  _$jscoverage['telerik.draganddrop.js'][90] = 0;
  _$jscoverage['telerik.draganddrop.js'][97] = 0;
  _$jscoverage['telerik.draganddrop.js'][101] = 0;
  _$jscoverage['telerik.draganddrop.js'][104] = 0;
  _$jscoverage['telerik.draganddrop.js'][106] = 0;
  _$jscoverage['telerik.draganddrop.js'][107] = 0;
  _$jscoverage['telerik.draganddrop.js'][109] = 0;
  _$jscoverage['telerik.draganddrop.js'][110] = 0;
  _$jscoverage['telerik.draganddrop.js'][112] = 0;
  _$jscoverage['telerik.draganddrop.js'][121] = 0;
  _$jscoverage['telerik.draganddrop.js'][127] = 0;
  _$jscoverage['telerik.draganddrop.js'][128] = 0;
  _$jscoverage['telerik.draganddrop.js'][133] = 0;
  _$jscoverage['telerik.draganddrop.js'][134] = 0;
  _$jscoverage['telerik.draganddrop.js'][138] = 0;
  _$jscoverage['telerik.draganddrop.js'][139] = 0;
  _$jscoverage['telerik.draganddrop.js'][140] = 0;
  _$jscoverage['telerik.draganddrop.js'][142] = 0;
  _$jscoverage['telerik.draganddrop.js'][143] = 0;
  _$jscoverage['telerik.draganddrop.js'][148] = 0;
  _$jscoverage['telerik.draganddrop.js'][153] = 0;
  _$jscoverage['telerik.draganddrop.js'][155] = 0;
  _$jscoverage['telerik.draganddrop.js'][156] = 0;
}
_$jscoverage['telerik.draganddrop.js'].source = ["<span class=\"k\">(</span><span class=\"k\">function</span> <span class=\"k\">(</span>$<span class=\"k\">)</span> <span class=\"k\">{</span>","    <span class=\"k\">var</span> $t <span class=\"k\">=</span> $<span class=\"k\">.</span>telerik<span class=\"k\">,</span>","        nop <span class=\"k\">=</span> <span class=\"k\">function</span> <span class=\"k\">()</span> <span class=\"k\">{</span> <span class=\"k\">}</span><span class=\"k\">,</span>","        draggables <span class=\"k\">=</span> <span class=\"k\">{}</span><span class=\"k\">,</span>","        cues <span class=\"k\">=</span> <span class=\"k\">{}</span><span class=\"k\">,</span>","        droppableDefaults <span class=\"k\">=</span> <span class=\"k\">{</span>","            scope<span class=\"k\">:</span> <span class=\"s\">'default'</span><span class=\"k\">,</span>","            drop<span class=\"k\">:</span> nop<span class=\"k\">,</span>","            over<span class=\"k\">:</span> nop<span class=\"k\">,</span>","            out<span class=\"k\">:</span> nop<span class=\"k\">,</span>","            owner<span class=\"k\">:</span> document<span class=\"k\">.</span>body","        <span class=\"k\">}</span><span class=\"k\">,</span>","        draggableDefaults <span class=\"k\">=</span> <span class=\"k\">{</span>","            distance<span class=\"k\">:</span> <span class=\"s\">5</span><span class=\"k\">,</span> <span class=\"c\">/* Dinstance in pixels the mouse should move before dragging should start. */</span>","            cursorAt<span class=\"k\">:</span> <span class=\"k\">{</span> left<span class=\"k\">:</span> <span class=\"s\">10</span><span class=\"k\">,</span> top<span class=\"k\">:</span> <span class=\"s\">10</span> <span class=\"k\">}</span><span class=\"k\">,</span> <span class=\"c\">/* The offset of the cursor from the dragging cue. */</span>","            scope<span class=\"k\">:</span> <span class=\"s\">'default'</span><span class=\"k\">,</span> <span class=\"c\">/* Used to group draggables and droppables. */</span>","            start<span class=\"k\">:</span> nop<span class=\"k\">,</span> <span class=\"c\">/* Called when dragging starts. Return `false` to prevent dragging. */</span>","            drag<span class=\"k\">:</span> nop<span class=\"k\">,</span> <span class=\"c\">/* Called when the mouse is moved during dragging. */</span>","            stop<span class=\"k\">:</span> nop<span class=\"k\">,</span> <span class=\"c\">/* Called when dragging stops. Return `false` to prevent the stop animation. */</span>","            destroy<span class=\"k\">:</span> nop<span class=\"k\">,</span> <span class=\"c\">/* Called when the draggable is destroyed. Used to remove any dragging/dropping cues from DOM. */</span>","            owner<span class=\"k\">:</span> document<span class=\"k\">.</span>body<span class=\"k\">,</span> <span class=\"c\">/* The DOM element to which events are attached. Used with 'selector' and 'delegate'. */</span>","            cue<span class=\"k\">:</span> <span class=\"k\">function</span><span class=\"k\">()</span> <span class=\"k\">{</span> <span class=\"c\">/* Called to create the dragging cue. Return a jQuery object representing the cue. */</span>","                <span class=\"k\">return</span> $<span class=\"k\">(</span><span class=\"s\">'&lt;span /&gt;'</span><span class=\"k\">);</span>","            <span class=\"k\">}</span>","        <span class=\"k\">}</span><span class=\"k\">;</span>    ","    ","    $t<span class=\"k\">.</span>droppable <span class=\"k\">=</span> <span class=\"k\">function</span> <span class=\"k\">(</span>options<span class=\"k\">)</span> <span class=\"k\">{</span>","       $<span class=\"k\">.</span>extend<span class=\"k\">(</span><span class=\"k\">this</span><span class=\"k\">,</span> droppableDefaults<span class=\"k\">,</span> options<span class=\"k\">);</span>","       $<span class=\"k\">(</span><span class=\"k\">this</span><span class=\"k\">.</span>owner<span class=\"k\">).</span>delegate<span class=\"k\">(</span><span class=\"k\">this</span><span class=\"k\">.</span>selector<span class=\"k\">,</span> <span class=\"s\">'mouseenter'</span><span class=\"k\">,</span> $<span class=\"k\">.</span>proxy<span class=\"k\">(</span><span class=\"k\">this</span><span class=\"k\">.</span>_over<span class=\"k\">,</span> <span class=\"k\">this</span><span class=\"k\">))</span>","                    <span class=\"k\">.</span>delegate<span class=\"k\">(</span><span class=\"k\">this</span><span class=\"k\">.</span>selector<span class=\"k\">,</span> <span class=\"s\">'mouseup'</span><span class=\"k\">,</span> $<span class=\"k\">.</span>proxy<span class=\"k\">(</span><span class=\"k\">this</span><span class=\"k\">.</span>_drop<span class=\"k\">,</span> <span class=\"k\">this</span><span class=\"k\">))</span>","                    <span class=\"k\">.</span>delegate<span class=\"k\">(</span><span class=\"k\">this</span><span class=\"k\">.</span>selector<span class=\"k\">,</span> <span class=\"s\">'mouseleave'</span><span class=\"k\">,</span> $<span class=\"k\">.</span>proxy<span class=\"k\">(</span><span class=\"k\">this</span><span class=\"k\">.</span>_out<span class=\"k\">,</span> <span class=\"k\">this</span><span class=\"k\">));</span>","    <span class=\"k\">}</span>","    ","    $t<span class=\"k\">.</span>droppable<span class=\"k\">.</span>prototype <span class=\"k\">=</span> <span class=\"k\">{</span>","        _over<span class=\"k\">:</span> <span class=\"k\">function</span><span class=\"k\">(</span>e<span class=\"k\">)</span> <span class=\"k\">{</span>","            <span class=\"k\">this</span><span class=\"k\">.</span>_raise<span class=\"k\">(</span>e<span class=\"k\">,</span> <span class=\"k\">this</span><span class=\"k\">.</span>over<span class=\"k\">);</span>","        <span class=\"k\">}</span><span class=\"k\">,</span>","        _out<span class=\"k\">:</span> <span class=\"k\">function</span><span class=\"k\">(</span>e<span class=\"k\">)</span> <span class=\"k\">{</span>","            <span class=\"k\">this</span><span class=\"k\">.</span>_raise<span class=\"k\">(</span>e<span class=\"k\">,</span> <span class=\"k\">this</span><span class=\"k\">.</span>out<span class=\"k\">);</span>","        <span class=\"k\">}</span><span class=\"k\">,</span>","        _drop<span class=\"k\">:</span> <span class=\"k\">function</span><span class=\"k\">(</span>e<span class=\"k\">)</span> <span class=\"k\">{</span>","            <span class=\"k\">this</span><span class=\"k\">.</span>_raise<span class=\"k\">(</span>e<span class=\"k\">,</span> $<span class=\"k\">.</span>proxy<span class=\"k\">(</span><span class=\"k\">function</span><span class=\"k\">(</span>e<span class=\"k\">)</span> <span class=\"k\">{</span>","                <span class=\"k\">this</span><span class=\"k\">.</span>drop<span class=\"k\">(</span>e<span class=\"k\">);</span>","                e<span class=\"k\">.</span>destroy<span class=\"k\">(</span>e<span class=\"k\">);</span>","            <span class=\"k\">}</span><span class=\"k\">,</span> <span class=\"k\">this</span><span class=\"k\">));</span>","        <span class=\"k\">}</span><span class=\"k\">,</span>","        _raise<span class=\"k\">:</span> <span class=\"k\">function</span><span class=\"k\">(</span>e<span class=\"k\">,</span> callback<span class=\"k\">)</span> <span class=\"k\">{</span>","            <span class=\"k\">var</span> draggable <span class=\"k\">=</span> draggables<span class=\"k\">[</span><span class=\"k\">this</span><span class=\"k\">.</span>scope<span class=\"k\">];</span>","            <span class=\"k\">if</span> <span class=\"k\">(</span>draggable<span class=\"k\">)</span>","                callback<span class=\"k\">(</span>$<span class=\"k\">.</span>extend<span class=\"k\">(</span>e<span class=\"k\">,</span> draggable<span class=\"k\">,</span> <span class=\"k\">{</span> $droppable<span class=\"k\">:</span> $<span class=\"k\">(</span>e<span class=\"k\">.</span>currentTarget<span class=\"k\">)</span> <span class=\"k\">}</span><span class=\"k\">));</span>","        <span class=\"k\">}</span>","    <span class=\"k\">}</span>","","    $t<span class=\"k\">.</span>dragCue <span class=\"k\">=</span> <span class=\"k\">function</span> <span class=\"k\">(</span>html<span class=\"k\">)</span> <span class=\"k\">{</span>","        <span class=\"k\">return</span> $<span class=\"k\">(</span><span class=\"s\">'&lt;div class=\"t-header t-drag-clue\" /&gt;'</span><span class=\"k\">)</span>","            <span class=\"k\">.</span>html<span class=\"k\">(</span>html<span class=\"k\">)</span>","            <span class=\"k\">.</span>prepend<span class=\"k\">(</span><span class=\"s\">'&lt;span class=\"t-icon t-drag-status t-denied\" /&gt;'</span><span class=\"k\">)</span>","            <span class=\"k\">.</span>appendTo<span class=\"k\">(</span>document<span class=\"k\">.</span>body<span class=\"k\">);</span>","    <span class=\"k\">}</span>","    ","    $t<span class=\"k\">.</span>dragCueStatus <span class=\"k\">=</span> <span class=\"k\">function</span><span class=\"k\">(</span>$cue<span class=\"k\">,</span> status<span class=\"k\">)</span> <span class=\"k\">{</span>","        $cue<span class=\"k\">.</span>find<span class=\"k\">(</span><span class=\"s\">'.t-drag-status'</span><span class=\"k\">)</span>","            <span class=\"k\">.</span>attr<span class=\"k\">(</span><span class=\"s\">'className'</span><span class=\"k\">,</span> <span class=\"s\">'t-icon t-drag-status'</span><span class=\"k\">)</span>","            <span class=\"k\">.</span>addClass<span class=\"k\">(</span>status<span class=\"k\">);</span>","    <span class=\"k\">}</span>","","    $t<span class=\"k\">.</span>draggable <span class=\"k\">=</span> <span class=\"k\">function</span> <span class=\"k\">(</span>options<span class=\"k\">)</span> <span class=\"k\">{</span>","        $<span class=\"k\">.</span>extend<span class=\"k\">(</span><span class=\"k\">this</span><span class=\"k\">,</span> draggableDefaults<span class=\"k\">,</span> options<span class=\"k\">);</span>","        ","        $<span class=\"k\">(</span><span class=\"k\">this</span><span class=\"k\">.</span>owner<span class=\"k\">).</span>delegate<span class=\"k\">(</span><span class=\"k\">this</span><span class=\"k\">.</span>selector<span class=\"k\">,</span> <span class=\"s\">'mousedown'</span><span class=\"k\">,</span> $<span class=\"k\">.</span>proxy<span class=\"k\">(</span><span class=\"k\">this</span><span class=\"k\">.</span>_wait<span class=\"k\">,</span> <span class=\"k\">this</span><span class=\"k\">))</span>","                     <span class=\"k\">.</span>delegate<span class=\"k\">(</span><span class=\"k\">this</span><span class=\"k\">.</span>selector<span class=\"k\">,</span> <span class=\"s\">'dragstart'</span><span class=\"k\">,</span> $t<span class=\"k\">.</span>preventDefault<span class=\"k\">);</span>","    ","        <span class=\"k\">this</span><span class=\"k\">.</span>_startProxy <span class=\"k\">=</span> $<span class=\"k\">.</span>proxy<span class=\"k\">(</span><span class=\"k\">this</span><span class=\"k\">.</span>_start<span class=\"k\">,</span> <span class=\"k\">this</span><span class=\"k\">);</span>","        <span class=\"k\">this</span><span class=\"k\">.</span>_destroyProxy <span class=\"k\">=</span> $<span class=\"k\">.</span>proxy<span class=\"k\">(</span><span class=\"k\">this</span><span class=\"k\">.</span>_destroy<span class=\"k\">,</span> <span class=\"k\">this</span><span class=\"k\">);</span>","        <span class=\"k\">this</span><span class=\"k\">.</span>_stopProxy <span class=\"k\">=</span> $<span class=\"k\">.</span>proxy<span class=\"k\">(</span><span class=\"k\">this</span><span class=\"k\">.</span>_stop<span class=\"k\">,</span> <span class=\"k\">this</span><span class=\"k\">);</span>","        <span class=\"k\">this</span><span class=\"k\">.</span>_dragProxy <span class=\"k\">=</span> $<span class=\"k\">.</span>proxy<span class=\"k\">(</span><span class=\"k\">this</span><span class=\"k\">.</span>_drag<span class=\"k\">,</span> <span class=\"k\">this</span><span class=\"k\">);</span>","    <span class=\"k\">}</span>","","    $t<span class=\"k\">.</span>draggable<span class=\"k\">.</span>prototype <span class=\"k\">=</span> <span class=\"k\">{</span>","        _raise<span class=\"k\">:</span> <span class=\"k\">function</span><span class=\"k\">(</span>e<span class=\"k\">,</span> callback<span class=\"k\">)</span> <span class=\"k\">{</span>","            <span class=\"k\">var</span> draggable <span class=\"k\">=</span> draggables<span class=\"k\">[</span><span class=\"k\">this</span><span class=\"k\">.</span>scope<span class=\"k\">];</span>","            <span class=\"k\">if</span> <span class=\"k\">(</span>draggable<span class=\"k\">)</span>","                <span class=\"k\">return</span> callback<span class=\"k\">(</span>$<span class=\"k\">.</span>extend<span class=\"k\">(</span>e<span class=\"k\">,</span> draggable<span class=\"k\">));</span>","        <span class=\"k\">}</span><span class=\"k\">,</span>","","        _wait<span class=\"k\">:</span> <span class=\"k\">function</span> <span class=\"k\">(</span>e<span class=\"k\">)</span> <span class=\"k\">{</span>","            <span class=\"k\">this</span><span class=\"k\">.</span>$target <span class=\"k\">=</span> $<span class=\"k\">(</span>e<span class=\"k\">.</span>currentTarget<span class=\"k\">);</span>","            <span class=\"k\">this</span><span class=\"k\">.</span>_startPosition <span class=\"k\">=</span> <span class=\"k\">{</span> x<span class=\"k\">:</span> e<span class=\"k\">.</span>pageX<span class=\"k\">,</span> y<span class=\"k\">:</span> e<span class=\"k\">.</span>pageY <span class=\"k\">}</span><span class=\"k\">;</span>","","            $<span class=\"k\">(</span>document<span class=\"k\">).</span>bind<span class=\"k\">(</span> <span class=\"k\">{</span>","                        mousemove<span class=\"k\">:</span> <span class=\"k\">this</span><span class=\"k\">.</span>_startProxy<span class=\"k\">,</span>","                        mouseup<span class=\"k\">:</span> <span class=\"k\">this</span><span class=\"k\">.</span>_destroyProxy","                      <span class=\"k\">}</span><span class=\"k\">)</span>","                      <span class=\"k\">.</span>trigger<span class=\"k\">(</span><span class=\"s\">'mousedown'</span><span class=\"k\">,</span> e<span class=\"k\">);</span> <span class=\"c\">// manually triggering 'mousedown' because the next statement will prevent that.</span>","","            <span class=\"c\">// required to avoid selection in Gecko</span>","            <span class=\"k\">return</span> <span class=\"k\">false</span><span class=\"k\">;</span>","        <span class=\"k\">}</span><span class=\"k\">,</span>","","        _start<span class=\"k\">:</span> <span class=\"k\">function</span><span class=\"k\">(</span>e<span class=\"k\">)</span> <span class=\"k\">{</span>","            <span class=\"k\">var</span> x <span class=\"k\">=</span> <span class=\"k\">this</span><span class=\"k\">.</span>_startPosition<span class=\"k\">.</span>x <span class=\"k\">-</span> e<span class=\"k\">.</span>pageX<span class=\"k\">,</span> ","                y <span class=\"k\">=</span> <span class=\"k\">this</span><span class=\"k\">.</span>_startPosition<span class=\"k\">.</span>y <span class=\"k\">-</span> e<span class=\"k\">.</span>pageY<span class=\"k\">;</span>","","            <span class=\"k\">var</span> distance <span class=\"k\">=</span> Math<span class=\"k\">.</span>sqrt<span class=\"k\">((</span>x <span class=\"k\">*</span> x<span class=\"k\">)</span> <span class=\"k\">+</span> <span class=\"k\">(</span>y <span class=\"k\">*</span> y<span class=\"k\">));</span>","            ","            <span class=\"k\">if</span> <span class=\"k\">(</span>distance <span class=\"k\">&gt;=</span> <span class=\"k\">this</span><span class=\"k\">.</span>distance<span class=\"k\">)</span> <span class=\"k\">{</span>","                <span class=\"k\">var</span> $cue <span class=\"k\">=</span> cues<span class=\"k\">[</span><span class=\"k\">this</span><span class=\"k\">.</span>selector<span class=\"k\">];</span>","                ","                <span class=\"k\">if</span> <span class=\"k\">(!</span>$cue<span class=\"k\">)</span> <span class=\"k\">{</span>","                    $cue <span class=\"k\">=</span> cues<span class=\"k\">[</span><span class=\"k\">this</span><span class=\"k\">.</span>selector<span class=\"k\">]</span> <span class=\"k\">=</span> <span class=\"k\">this</span><span class=\"k\">.</span>cue<span class=\"k\">(</span><span class=\"k\">{</span> $draggable<span class=\"k\">:</span> <span class=\"k\">this</span><span class=\"k\">.</span>$target <span class=\"k\">}</span><span class=\"k\">);</span>","                    ","                    $<span class=\"k\">(</span>document<span class=\"k\">).</span>unbind<span class=\"k\">(</span><span class=\"s\">'mousemove'</span><span class=\"k\">,</span> <span class=\"k\">this</span><span class=\"k\">.</span>_startProxy<span class=\"k\">)</span>","                               <span class=\"k\">.</span>unbind<span class=\"k\">(</span><span class=\"s\">'mouseup'</span><span class=\"k\">,</span> <span class=\"k\">this</span><span class=\"k\">.</span>_destroyProxy<span class=\"k\">)</span>","                               <span class=\"k\">.</span>bind<span class=\"k\">(</span><span class=\"k\">{</span>","                                <span class=\"s\">'mouseup keydown'</span><span class=\"k\">:</span> <span class=\"k\">this</span><span class=\"k\">.</span>_stopProxy<span class=\"k\">,</span>","                                mousemove<span class=\"k\">:</span> <span class=\"k\">this</span><span class=\"k\">.</span>_dragProxy<span class=\"k\">,</span>","                                selectstart<span class=\"k\">:</span> <span class=\"k\">false</span>","                                <span class=\"k\">}</span><span class=\"k\">);</span>","                <span class=\"k\">}</span> ","                    ","                draggables<span class=\"k\">[</span><span class=\"k\">this</span><span class=\"k\">.</span>scope<span class=\"k\">]</span> <span class=\"k\">=</span> <span class=\"k\">{</span>","                    $cue<span class=\"k\">:</span> $cue<span class=\"k\">.</span>css<span class=\"k\">(</span><span class=\"k\">{</span> position<span class=\"k\">:</span> <span class=\"s\">'absolute'</span><span class=\"k\">,</span> left<span class=\"k\">:</span> e<span class=\"k\">.</span>pageX <span class=\"k\">+</span> <span class=\"k\">this</span><span class=\"k\">.</span>cursorAt<span class=\"k\">.</span>left<span class=\"k\">,</span> top<span class=\"k\">:</span> e<span class=\"k\">.</span>pageY <span class=\"k\">+</span> <span class=\"k\">this</span><span class=\"k\">.</span>cursorAt<span class=\"k\">.</span>top <span class=\"k\">}</span><span class=\"k\">),</span>","                    $draggable<span class=\"k\">:</span> <span class=\"k\">this</span><span class=\"k\">.</span>$target<span class=\"k\">,</span>","                    destroy<span class=\"k\">:</span> <span class=\"k\">this</span><span class=\"k\">.</span>_destroyProxy","                <span class=\"k\">}</span>","","                <span class=\"k\">if</span> <span class=\"k\">(</span><span class=\"k\">this</span><span class=\"k\">.</span>_raise<span class=\"k\">(</span>e<span class=\"k\">,</span> <span class=\"k\">this</span><span class=\"k\">.</span>start<span class=\"k\">)</span> <span class=\"k\">===</span> <span class=\"k\">false</span><span class=\"k\">)</span>","                    <span class=\"k\">this</span><span class=\"k\">.</span>_destroy<span class=\"k\">(</span>e<span class=\"k\">);</span>","            <span class=\"k\">}</span>","        <span class=\"k\">}</span><span class=\"k\">,</span>","","        _drag<span class=\"k\">:</span> <span class=\"k\">function</span><span class=\"k\">(</span>e<span class=\"k\">)</span> <span class=\"k\">{</span>","            <span class=\"k\">this</span><span class=\"k\">.</span>_raise<span class=\"k\">(</span>e<span class=\"k\">,</span> <span class=\"k\">this</span><span class=\"k\">.</span>drag<span class=\"k\">);</span>","            draggables<span class=\"k\">[</span><span class=\"k\">this</span><span class=\"k\">.</span>scope<span class=\"k\">].</span>$cue<span class=\"k\">.</span>css<span class=\"k\">(</span><span class=\"k\">{</span> left<span class=\"k\">:</span> e<span class=\"k\">.</span>pageX <span class=\"k\">+</span> <span class=\"k\">this</span><span class=\"k\">.</span>cursorAt<span class=\"k\">.</span>left<span class=\"k\">,</span> top<span class=\"k\">:</span> e<span class=\"k\">.</span>pageY <span class=\"k\">+</span> <span class=\"k\">this</span><span class=\"k\">.</span>cursorAt<span class=\"k\">.</span>top <span class=\"k\">}</span><span class=\"k\">);</span>","        <span class=\"k\">}</span><span class=\"k\">,</span>","","        _stop<span class=\"k\">:</span> <span class=\"k\">function</span><span class=\"k\">(</span>e<span class=\"k\">)</span> <span class=\"k\">{</span>","            <span class=\"k\">if</span> <span class=\"k\">(</span>e<span class=\"k\">.</span>type <span class=\"k\">==</span> <span class=\"s\">'mouseup'</span> <span class=\"k\">||</span> e<span class=\"k\">.</span>keyCode <span class=\"k\">==</span> <span class=\"s\">27</span><span class=\"k\">)</span>","                <span class=\"k\">if</span> <span class=\"k\">(</span><span class=\"k\">this</span><span class=\"k\">.</span>_raise<span class=\"k\">(</span>e<span class=\"k\">,</span> <span class=\"k\">this</span><span class=\"k\">.</span>stop<span class=\"k\">)</span> <span class=\"k\">===</span> <span class=\"k\">false</span><span class=\"k\">)</span> <span class=\"k\">{</span>","                    <span class=\"k\">this</span><span class=\"k\">.</span>_destroy<span class=\"k\">(</span>e<span class=\"k\">);</span>","                <span class=\"k\">}</span> <span class=\"k\">else</span> <span class=\"k\">{</span>","                    <span class=\"k\">var</span> draggable <span class=\"k\">=</span> draggables<span class=\"k\">[</span><span class=\"k\">this</span><span class=\"k\">.</span>scope<span class=\"k\">];</span>","                    draggable<span class=\"k\">.</span>$cue<span class=\"k\">.</span>animate<span class=\"k\">(</span>draggable<span class=\"k\">.</span>$draggable<span class=\"k\">.</span>offset<span class=\"k\">(),</span> <span class=\"s\">'fast'</span><span class=\"k\">,</span> <span class=\"k\">this</span><span class=\"k\">.</span>_destroyProxy<span class=\"k\">);</span>","                <span class=\"k\">}</span>","        <span class=\"k\">}</span><span class=\"k\">,</span>","","        _destroy<span class=\"k\">:</span> <span class=\"k\">function</span><span class=\"k\">(</span>e<span class=\"k\">)</span> <span class=\"k\">{</span>","            $<span class=\"k\">(</span>document<span class=\"k\">).</span>unbind<span class=\"k\">(</span><span class=\"s\">'mouseup keydown'</span><span class=\"k\">,</span> <span class=\"k\">this</span><span class=\"k\">.</span>_stopProxy<span class=\"k\">)</span>","                       <span class=\"k\">.</span>unbind<span class=\"k\">(</span><span class=\"s\">'mousemove'</span><span class=\"k\">,</span> <span class=\"k\">this</span><span class=\"k\">.</span>_dragProxy<span class=\"k\">)</span>","                       <span class=\"k\">.</span>unbind<span class=\"k\">(</span><span class=\"s\">'mousemove'</span><span class=\"k\">,</span> <span class=\"k\">this</span><span class=\"k\">.</span>_startProxy<span class=\"k\">)</span>","                       <span class=\"k\">.</span>unbind<span class=\"k\">(</span><span class=\"s\">'selectstart'</span><span class=\"k\">,</span> <span class=\"k\">false</span><span class=\"k\">);</span>","            ","            <span class=\"k\">this</span><span class=\"k\">.</span>_raise<span class=\"k\">(</span>e<span class=\"k\">,</span> <span class=\"k\">this</span><span class=\"k\">.</span>destroy<span class=\"k\">);</span>","","            draggables<span class=\"k\">[</span><span class=\"k\">this</span><span class=\"k\">.</span>scope<span class=\"k\">]</span> <span class=\"k\">=</span> <span class=\"k\">null</span><span class=\"k\">;</span>","            cues<span class=\"k\">[</span><span class=\"k\">this</span><span class=\"k\">.</span>selector<span class=\"k\">]</span> <span class=\"k\">=</span> <span class=\"k\">null</span><span class=\"k\">;</span>","        <span class=\"k\">}</span>","    <span class=\"k\">}</span>","<span class=\"k\">}</span><span class=\"k\">)(</span>jQuery<span class=\"k\">);</span>"];
_$jscoverage['telerik.draganddrop.js'][1]++;
(function ($) {
  _$jscoverage['telerik.draganddrop.js'][2]++;
  var $t = $.telerik, nop = (function () {
}), draggables = {}, cues = {}, droppableDefaults = {scope: "default", drop: nop, over: nop, out: nop, owner: document.body}, draggableDefaults = {distance: 5, cursorAt: {left: 10, top: 10}, scope: "default", start: nop, drag: nop, stop: nop, destroy: nop, owner: document.body, cue: (function () {
  _$jscoverage['telerik.draganddrop.js'][23]++;
  return $("<span />");
})};
  _$jscoverage['telerik.draganddrop.js'][27]++;
  $t.droppable = (function (options) {
  _$jscoverage['telerik.draganddrop.js'][28]++;
  $.extend(this, droppableDefaults, options);
  _$jscoverage['telerik.draganddrop.js'][29]++;
  $(this.owner).delegate(this.selector, "mouseenter", $.proxy(this._over, this)).delegate(this.selector, "mouseup", $.proxy(this._drop, this)).delegate(this.selector, "mouseleave", $.proxy(this._out, this));
});
  _$jscoverage['telerik.draganddrop.js'][34]++;
  $t.droppable.prototype = {_over: (function (e) {
  _$jscoverage['telerik.draganddrop.js'][36]++;
  this._raise(e, this.over);
}), _out: (function (e) {
  _$jscoverage['telerik.draganddrop.js'][39]++;
  this._raise(e, this.out);
}), _drop: (function (e) {
  _$jscoverage['telerik.draganddrop.js'][42]++;
  this._raise(e, $.proxy((function (e) {
  _$jscoverage['telerik.draganddrop.js'][43]++;
  this.drop(e);
  _$jscoverage['telerik.draganddrop.js'][44]++;
  e.destroy(e);
}), this));
}), _raise: (function (e, callback) {
  _$jscoverage['telerik.draganddrop.js'][48]++;
  var draggable = draggables[this.scope];
  _$jscoverage['telerik.draganddrop.js'][49]++;
  if (draggable) {
    _$jscoverage['telerik.draganddrop.js'][50]++;
    callback($.extend(e, draggable, {$droppable: $(e.currentTarget)}));
  }
})};
  _$jscoverage['telerik.draganddrop.js'][54]++;
  $t.dragCue = (function (html) {
  _$jscoverage['telerik.draganddrop.js'][55]++;
  return $("<div class=\"t-header t-drag-clue\" />").html(html).prepend("<span class=\"t-icon t-drag-status t-denied\" />").appendTo(document.body);
});
  _$jscoverage['telerik.draganddrop.js'][61]++;
  $t.dragCueStatus = (function ($cue, status) {
  _$jscoverage['telerik.draganddrop.js'][62]++;
  $cue.find(".t-drag-status").attr("className", "t-icon t-drag-status").addClass(status);
});
  _$jscoverage['telerik.draganddrop.js'][67]++;
  $t.draggable = (function (options) {
  _$jscoverage['telerik.draganddrop.js'][68]++;
  $.extend(this, draggableDefaults, options);
  _$jscoverage['telerik.draganddrop.js'][70]++;
  $(this.owner).delegate(this.selector, "mousedown", $.proxy(this._wait, this)).delegate(this.selector, "dragstart", $t.preventDefault);
  _$jscoverage['telerik.draganddrop.js'][73]++;
  this._startProxy = $.proxy(this._start, this);
  _$jscoverage['telerik.draganddrop.js'][74]++;
  this._destroyProxy = $.proxy(this._destroy, this);
  _$jscoverage['telerik.draganddrop.js'][75]++;
  this._stopProxy = $.proxy(this._stop, this);
  _$jscoverage['telerik.draganddrop.js'][76]++;
  this._dragProxy = $.proxy(this._drag, this);
});
  _$jscoverage['telerik.draganddrop.js'][79]++;
  $t.draggable.prototype = {_raise: (function (e, callback) {
  _$jscoverage['telerik.draganddrop.js'][81]++;
  var draggable = draggables[this.scope];
  _$jscoverage['telerik.draganddrop.js'][82]++;
  if (draggable) {
    _$jscoverage['telerik.draganddrop.js'][83]++;
    return callback($.extend(e, draggable));
  }
}), _wait: (function (e) {
  _$jscoverage['telerik.draganddrop.js'][87]++;
  this.$target = $(e.currentTarget);
  _$jscoverage['telerik.draganddrop.js'][88]++;
  this._startPosition = {x: e.pageX, y: e.pageY};
  _$jscoverage['telerik.draganddrop.js'][90]++;
  $(document).bind({mousemove: this._startProxy, mouseup: this._destroyProxy}).trigger("mousedown", e);
  _$jscoverage['telerik.draganddrop.js'][97]++;
  return false;
}), _start: (function (e) {
  _$jscoverage['telerik.draganddrop.js'][101]++;
  var x = (this._startPosition.x - e.pageX), y = (this._startPosition.y - e.pageY);
  _$jscoverage['telerik.draganddrop.js'][104]++;
  var distance = Math.sqrt(((x * x) + (y * y)));
  _$jscoverage['telerik.draganddrop.js'][106]++;
  if ((distance >= this.distance)) {
    _$jscoverage['telerik.draganddrop.js'][107]++;
    var $cue = cues[this.selector];
    _$jscoverage['telerik.draganddrop.js'][109]++;
    if ((! $cue)) {
      _$jscoverage['telerik.draganddrop.js'][110]++;
      $cue = (cues[this.selector] = this.cue({$draggable: this.$target}));
      _$jscoverage['telerik.draganddrop.js'][112]++;
      $(document).unbind("mousemove", this._startProxy).unbind("mouseup", this._destroyProxy).bind({"mouseup keydown": this._stopProxy, mousemove: this._dragProxy, selectstart: false});
    }
    _$jscoverage['telerik.draganddrop.js'][121]++;
    draggables[this.scope] = {$cue: $cue.css({position: "absolute", left: (e.pageX + this.cursorAt.left), top: (e.pageY + this.cursorAt.top)}), $draggable: this.$target, destroy: this._destroyProxy};
    _$jscoverage['telerik.draganddrop.js'][127]++;
    if ((this._raise(e, this.start) === false)) {
      _$jscoverage['telerik.draganddrop.js'][128]++;
      this._destroy(e);
    }
  }
}), _drag: (function (e) {
  _$jscoverage['telerik.draganddrop.js'][133]++;
  this._raise(e, this.drag);
  _$jscoverage['telerik.draganddrop.js'][134]++;
  draggables[this.scope].$cue.css({left: (e.pageX + this.cursorAt.left), top: (e.pageY + this.cursorAt.top)});
}), _stop: (function (e) {
  _$jscoverage['telerik.draganddrop.js'][138]++;
  if (((e.type == "mouseup") || (e.keyCode == 27))) {
    _$jscoverage['telerik.draganddrop.js'][139]++;
    if ((this._raise(e, this.stop) === false)) {
      _$jscoverage['telerik.draganddrop.js'][140]++;
      this._destroy(e);
    }
    else {
      _$jscoverage['telerik.draganddrop.js'][142]++;
      var draggable = draggables[this.scope];
      _$jscoverage['telerik.draganddrop.js'][143]++;
      draggable.$cue.animate(draggable.$draggable.offset(), "fast", this._destroyProxy);
    }
  }
}), _destroy: (function (e) {
  _$jscoverage['telerik.draganddrop.js'][148]++;
  $(document).unbind("mouseup keydown", this._stopProxy).unbind("mousemove", this._dragProxy).unbind("mousemove", this._startProxy).unbind("selectstart", false);
  _$jscoverage['telerik.draganddrop.js'][153]++;
  this._raise(e, this.destroy);
  _$jscoverage['telerik.draganddrop.js'][155]++;
  draggables[this.scope] = null;
  _$jscoverage['telerik.draganddrop.js'][156]++;
  cues[this.selector] = null;
})};
})(jQuery);
