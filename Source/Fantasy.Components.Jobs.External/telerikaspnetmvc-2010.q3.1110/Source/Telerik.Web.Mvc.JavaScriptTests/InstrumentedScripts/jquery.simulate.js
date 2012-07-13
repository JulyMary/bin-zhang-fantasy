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
if (! _$jscoverage['jquery.simulate.js']) {
  _$jscoverage['jquery.simulate.js'] = [];
  _$jscoverage['jquery.simulate.js'][13] = 0;
  _$jscoverage['jquery.simulate.js'][15] = 0;
  _$jscoverage['jquery.simulate.js'][17] = 0;
  _$jscoverage['jquery.simulate.js'][18] = 0;
  _$jscoverage['jquery.simulate.js'][19] = 0;
  _$jscoverage['jquery.simulate.js'][24] = 0;
  _$jscoverage['jquery.simulate.js'][25] = 0;
  _$jscoverage['jquery.simulate.js'][26] = 0;
  _$jscoverage['jquery.simulate.js'][28] = 0;
  _$jscoverage['jquery.simulate.js'][29] = 0;
  _$jscoverage['jquery.simulate.js'][31] = 0;
  _$jscoverage['jquery.simulate.js'][35] = 0;
  _$jscoverage['jquery.simulate.js'][37] = 0;
  _$jscoverage['jquery.simulate.js'][38] = 0;
  _$jscoverage['jquery.simulate.js'][39] = 0;
  _$jscoverage['jquery.simulate.js'][42] = 0;
  _$jscoverage['jquery.simulate.js'][43] = 0;
  _$jscoverage['jquery.simulate.js'][44] = 0;
  _$jscoverage['jquery.simulate.js'][45] = 0;
  _$jscoverage['jquery.simulate.js'][49] = 0;
  _$jscoverage['jquery.simulate.js'][50] = 0;
  _$jscoverage['jquery.simulate.js'][58] = 0;
  _$jscoverage['jquery.simulate.js'][60] = 0;
  _$jscoverage['jquery.simulate.js'][61] = 0;
  _$jscoverage['jquery.simulate.js'][62] = 0;
  _$jscoverage['jquery.simulate.js'][66] = 0;
  _$jscoverage['jquery.simulate.js'][67] = 0;
  _$jscoverage['jquery.simulate.js'][68] = 0;
  _$jscoverage['jquery.simulate.js'][69] = 0;
  _$jscoverage['jquery.simulate.js'][71] = 0;
  _$jscoverage['jquery.simulate.js'][74] = 0;
  _$jscoverage['jquery.simulate.js'][76] = 0;
  _$jscoverage['jquery.simulate.js'][81] = 0;
  _$jscoverage['jquery.simulate.js'][82] = 0;
  _$jscoverage['jquery.simulate.js'][83] = 0;
  _$jscoverage['jquery.simulate.js'][84] = 0;
  _$jscoverage['jquery.simulate.js'][88] = 0;
  _$jscoverage['jquery.simulate.js'][89] = 0;
  _$jscoverage['jquery.simulate.js'][90] = 0;
  _$jscoverage['jquery.simulate.js'][95] = 0;
  _$jscoverage['jquery.simulate.js'][96] = 0;
  _$jscoverage['jquery.simulate.js'][97] = 0;
  _$jscoverage['jquery.simulate.js'][99] = 0;
  _$jscoverage['jquery.simulate.js'][100] = 0;
  _$jscoverage['jquery.simulate.js'][101] = 0;
  _$jscoverage['jquery.simulate.js'][103] = 0;
  _$jscoverage['jquery.simulate.js'][107] = 0;
  _$jscoverage['jquery.simulate.js'][108] = 0;
  _$jscoverage['jquery.simulate.js'][109] = 0;
  _$jscoverage['jquery.simulate.js'][110] = 0;
  _$jscoverage['jquery.simulate.js'][112] = 0;
  _$jscoverage['jquery.simulate.js'][116] = 0;
  _$jscoverage['jquery.simulate.js'][119] = 0;
  _$jscoverage['jquery.simulate.js'][120] = 0;
  _$jscoverage['jquery.simulate.js'][121] = 0;
  _$jscoverage['jquery.simulate.js'][122] = 0;
  _$jscoverage['jquery.simulate.js'][123] = 0;
  _$jscoverage['jquery.simulate.js'][124] = 0;
  _$jscoverage['jquery.simulate.js'][125] = 0;
  _$jscoverage['jquery.simulate.js'][126] = 0;
  _$jscoverage['jquery.simulate.js'][129] = 0;
  _$jscoverage['jquery.simulate.js'][130] = 0;
  _$jscoverage['jquery.simulate.js'][137] = 0;
}
_$jscoverage['jquery.simulate.js'].source = ["","<span class=\"c\">/*</span>","<span class=\"c\">* jquery.simulate - simulate browser mouse and keyboard events</span>","<span class=\"c\">*</span>","<span class=\"c\">* Copyright (c) 2009 Eduardo Lundgren (eduardolundgren@gmail.com)</span>","<span class=\"c\">* and Richard D. Worth (rdworth@gmail.com)</span>","<span class=\"c\">*</span>","<span class=\"c\">* Dual licensed under the MIT (http://www.opensource.org/licenses/mit-license.php)</span>","<span class=\"c\">* and GPL (http://www.opensource.org/licenses/gpl-license.php) licenses.</span>","<span class=\"c\">*</span>","<span class=\"c\">*/</span>","","<span class=\"k\">(</span><span class=\"k\">function</span> <span class=\"k\">(</span>$<span class=\"k\">)</span> <span class=\"k\">{</span>","","    $<span class=\"k\">.</span>fn<span class=\"k\">.</span>extend<span class=\"k\">(</span><span class=\"k\">{</span>","        simulate<span class=\"k\">:</span> <span class=\"k\">function</span> <span class=\"k\">(</span>type<span class=\"k\">,</span> options<span class=\"k\">)</span> <span class=\"k\">{</span>","            <span class=\"k\">return</span> <span class=\"k\">this</span><span class=\"k\">.</span>each<span class=\"k\">(</span><span class=\"k\">function</span> <span class=\"k\">()</span> <span class=\"k\">{</span>","                <span class=\"k\">var</span> opt <span class=\"k\">=</span> $<span class=\"k\">.</span>extend<span class=\"k\">(</span><span class=\"k\">{}</span><span class=\"k\">,</span> $<span class=\"k\">.</span>simulate<span class=\"k\">.</span>defaults<span class=\"k\">,</span> options <span class=\"k\">||</span> <span class=\"k\">{}</span><span class=\"k\">);</span>","                <span class=\"k\">new</span> $<span class=\"k\">.</span>simulate<span class=\"k\">(</span><span class=\"k\">this</span><span class=\"k\">,</span> type<span class=\"k\">,</span> opt<span class=\"k\">);</span>","            <span class=\"k\">}</span><span class=\"k\">);</span>","        <span class=\"k\">}</span>","    <span class=\"k\">}</span><span class=\"k\">);</span>","","    $<span class=\"k\">.</span>simulate <span class=\"k\">=</span> <span class=\"k\">function</span> <span class=\"k\">(</span>el<span class=\"k\">,</span> type<span class=\"k\">,</span> options<span class=\"k\">)</span> <span class=\"k\">{</span>","        <span class=\"k\">this</span><span class=\"k\">.</span>target <span class=\"k\">=</span> el<span class=\"k\">;</span>","        <span class=\"k\">this</span><span class=\"k\">.</span>options <span class=\"k\">=</span> options<span class=\"k\">;</span>","","        <span class=\"k\">if</span> <span class=\"k\">(</span><span class=\"s\">/^drag$/</span><span class=\"k\">.</span>test<span class=\"k\">(</span>type<span class=\"k\">))</span> <span class=\"k\">{</span>","            <span class=\"k\">this</span><span class=\"k\">[</span>type<span class=\"k\">].</span>apply<span class=\"k\">(</span><span class=\"k\">this</span><span class=\"k\">,</span> <span class=\"k\">[</span><span class=\"k\">this</span><span class=\"k\">.</span>target<span class=\"k\">,</span> options<span class=\"k\">]);</span>","        <span class=\"k\">}</span> <span class=\"k\">else</span> <span class=\"k\">{</span>","            <span class=\"k\">this</span><span class=\"k\">.</span>simulateEvent<span class=\"k\">(</span>el<span class=\"k\">,</span> type<span class=\"k\">,</span> options<span class=\"k\">);</span>","        <span class=\"k\">}</span>","    <span class=\"k\">}</span>","","    $<span class=\"k\">.</span>extend<span class=\"k\">(</span>$<span class=\"k\">.</span>simulate<span class=\"k\">.</span>prototype<span class=\"k\">,</span> <span class=\"k\">{</span>","        simulateEvent<span class=\"k\">:</span> <span class=\"k\">function</span> <span class=\"k\">(</span>el<span class=\"k\">,</span> type<span class=\"k\">,</span> options<span class=\"k\">)</span> <span class=\"k\">{</span>","            <span class=\"k\">var</span> evt <span class=\"k\">=</span> <span class=\"k\">this</span><span class=\"k\">.</span>createEvent<span class=\"k\">(</span>type<span class=\"k\">,</span> options<span class=\"k\">);</span>","            <span class=\"k\">this</span><span class=\"k\">.</span>dispatchEvent<span class=\"k\">(</span>el<span class=\"k\">,</span> type<span class=\"k\">,</span> evt<span class=\"k\">,</span> options<span class=\"k\">);</span>","            <span class=\"k\">return</span> evt<span class=\"k\">;</span>","        <span class=\"k\">}</span><span class=\"k\">,</span>","        createEvent<span class=\"k\">:</span> <span class=\"k\">function</span> <span class=\"k\">(</span>type<span class=\"k\">,</span> options<span class=\"k\">)</span> <span class=\"k\">{</span>","            <span class=\"k\">if</span> <span class=\"k\">(</span><span class=\"s\">/^mouse(over|out|down|up|move)|(dbl)?click$/</span><span class=\"k\">.</span>test<span class=\"k\">(</span>type<span class=\"k\">))</span> <span class=\"k\">{</span>","                <span class=\"k\">return</span> <span class=\"k\">this</span><span class=\"k\">.</span>mouseEvent<span class=\"k\">(</span>type<span class=\"k\">,</span> options<span class=\"k\">);</span>","            <span class=\"k\">}</span> <span class=\"k\">else</span> <span class=\"k\">if</span> <span class=\"k\">(</span><span class=\"s\">/^key(up|down|press)$/</span><span class=\"k\">.</span>test<span class=\"k\">(</span>type<span class=\"k\">))</span> <span class=\"k\">{</span>","                <span class=\"k\">return</span> <span class=\"k\">this</span><span class=\"k\">.</span>keyboardEvent<span class=\"k\">(</span>type<span class=\"k\">,</span> options<span class=\"k\">);</span>","            <span class=\"k\">}</span>","        <span class=\"k\">}</span><span class=\"k\">,</span>","        mouseEvent<span class=\"k\">:</span> <span class=\"k\">function</span> <span class=\"k\">(</span>type<span class=\"k\">,</span> options<span class=\"k\">)</span> <span class=\"k\">{</span>","            <span class=\"k\">var</span> evt<span class=\"k\">;</span>","            <span class=\"k\">var</span> e <span class=\"k\">=</span> $<span class=\"k\">.</span>extend<span class=\"k\">(</span><span class=\"k\">{</span>","                which<span class=\"k\">:</span> <span class=\"s\">1</span><span class=\"k\">,</span>","                bubbles<span class=\"k\">:</span> <span class=\"k\">true</span><span class=\"k\">,</span> cancelable<span class=\"k\">:</span> <span class=\"k\">(</span>type <span class=\"k\">!=</span> <span class=\"s\">\"mousemove\"</span><span class=\"k\">),</span> view<span class=\"k\">:</span> window<span class=\"k\">,</span> detail<span class=\"k\">:</span> <span class=\"s\">0</span><span class=\"k\">,</span>","                screenX<span class=\"k\">:</span> <span class=\"s\">0</span><span class=\"k\">,</span> screenY<span class=\"k\">:</span> <span class=\"s\">0</span><span class=\"k\">,</span> clientX<span class=\"k\">:</span> <span class=\"s\">0</span><span class=\"k\">,</span> clientY<span class=\"k\">:</span> <span class=\"s\">0</span><span class=\"k\">,</span>","                ctrlKey<span class=\"k\">:</span> <span class=\"k\">false</span><span class=\"k\">,</span> altKey<span class=\"k\">:</span> <span class=\"k\">false</span><span class=\"k\">,</span> shiftKey<span class=\"k\">:</span> <span class=\"k\">false</span><span class=\"k\">,</span> metaKey<span class=\"k\">:</span> <span class=\"k\">false</span><span class=\"k\">,</span>","                button<span class=\"k\">:</span> <span class=\"s\">0</span><span class=\"k\">,</span> relatedTarget<span class=\"k\">:</span> undefined","            <span class=\"k\">}</span><span class=\"k\">,</span> options<span class=\"k\">);</span>","","            <span class=\"k\">var</span> relatedTarget <span class=\"k\">=</span> $<span class=\"k\">(</span>e<span class=\"k\">.</span>relatedTarget<span class=\"k\">)[</span><span class=\"s\">0</span><span class=\"k\">];</span>","","            <span class=\"k\">if</span> <span class=\"k\">(</span>$<span class=\"k\">.</span>isFunction<span class=\"k\">(</span>document<span class=\"k\">.</span>createEvent<span class=\"k\">))</span> <span class=\"k\">{</span>","                evt <span class=\"k\">=</span> document<span class=\"k\">.</span>createEvent<span class=\"k\">(</span><span class=\"s\">\"MouseEvents\"</span><span class=\"k\">);</span>","                evt<span class=\"k\">.</span>initMouseEvent<span class=\"k\">(</span>type<span class=\"k\">,</span> e<span class=\"k\">.</span>bubbles<span class=\"k\">,</span> e<span class=\"k\">.</span>cancelable<span class=\"k\">,</span> e<span class=\"k\">.</span>view<span class=\"k\">,</span> e<span class=\"k\">.</span>detail<span class=\"k\">,</span>","                        e<span class=\"k\">.</span>screenX<span class=\"k\">,</span> e<span class=\"k\">.</span>screenY<span class=\"k\">,</span> e<span class=\"k\">.</span>clientX<span class=\"k\">,</span> e<span class=\"k\">.</span>clientY<span class=\"k\">,</span>","                        e<span class=\"k\">.</span>ctrlKey<span class=\"k\">,</span> e<span class=\"k\">.</span>altKey<span class=\"k\">,</span> e<span class=\"k\">.</span>shiftKey<span class=\"k\">,</span> e<span class=\"k\">.</span>metaKey<span class=\"k\">,</span>","                        e<span class=\"k\">.</span>button<span class=\"k\">,</span> e<span class=\"k\">.</span>relatedTarget <span class=\"k\">||</span> document<span class=\"k\">.</span>body<span class=\"k\">.</span>parentNode<span class=\"k\">);</span>","            <span class=\"k\">}</span> <span class=\"k\">else</span> <span class=\"k\">if</span> <span class=\"k\">(</span>document<span class=\"k\">.</span>createEventObject<span class=\"k\">)</span> <span class=\"k\">{</span>","                evt <span class=\"k\">=</span> document<span class=\"k\">.</span>createEventObject<span class=\"k\">();</span>","                $<span class=\"k\">.</span>extend<span class=\"k\">(</span>evt<span class=\"k\">,</span> e<span class=\"k\">);</span>","                evt<span class=\"k\">.</span>button <span class=\"k\">=</span> <span class=\"k\">{</span> <span class=\"s\">0</span><span class=\"k\">:</span> <span class=\"s\">1</span><span class=\"k\">,</span> <span class=\"s\">1</span><span class=\"k\">:</span> <span class=\"s\">4</span><span class=\"k\">,</span> <span class=\"s\">2</span><span class=\"k\">:</span> <span class=\"s\">2</span><span class=\"k\">}</span><span class=\"k\">[</span>evt<span class=\"k\">.</span>button<span class=\"k\">]</span> <span class=\"k\">||</span> evt<span class=\"k\">.</span>button<span class=\"k\">;</span>","            <span class=\"k\">}</span>","            <span class=\"k\">return</span> evt<span class=\"k\">;</span>","        <span class=\"k\">}</span><span class=\"k\">,</span>","        keyboardEvent<span class=\"k\">:</span> <span class=\"k\">function</span> <span class=\"k\">(</span>type<span class=\"k\">,</span> options<span class=\"k\">)</span> <span class=\"k\">{</span>","            <span class=\"k\">var</span> evt<span class=\"k\">;</span>","","            <span class=\"k\">var</span> e <span class=\"k\">=</span> $<span class=\"k\">.</span>extend<span class=\"k\">(</span><span class=\"k\">{</span> bubbles<span class=\"k\">:</span> <span class=\"k\">true</span><span class=\"k\">,</span> cancelable<span class=\"k\">:</span> <span class=\"k\">true</span><span class=\"k\">,</span> view<span class=\"k\">:</span> window<span class=\"k\">,</span>","                ctrlKey<span class=\"k\">:</span> <span class=\"k\">false</span><span class=\"k\">,</span> altKey<span class=\"k\">:</span> <span class=\"k\">false</span><span class=\"k\">,</span> shiftKey<span class=\"k\">:</span> <span class=\"k\">false</span><span class=\"k\">,</span> metaKey<span class=\"k\">:</span> <span class=\"k\">false</span><span class=\"k\">,</span>","                keyCode<span class=\"k\">:</span> <span class=\"s\">0</span><span class=\"k\">,</span> charCode<span class=\"k\">:</span> <span class=\"s\">0</span>","            <span class=\"k\">}</span><span class=\"k\">,</span> options<span class=\"k\">);</span>","","            <span class=\"k\">if</span> <span class=\"k\">(</span>$<span class=\"k\">.</span>isFunction<span class=\"k\">(</span>document<span class=\"k\">.</span>createEvent<span class=\"k\">))</span> <span class=\"k\">{</span>","                <span class=\"k\">try</span> <span class=\"k\">{</span>","                    evt <span class=\"k\">=</span> document<span class=\"k\">.</span>createEvent<span class=\"k\">(</span><span class=\"s\">\"KeyEvents\"</span><span class=\"k\">);</span>","                    evt<span class=\"k\">.</span>initKeyEvent<span class=\"k\">(</span>type<span class=\"k\">,</span> e<span class=\"k\">.</span>bubbles<span class=\"k\">,</span> e<span class=\"k\">.</span>cancelable<span class=\"k\">,</span> e<span class=\"k\">.</span>view<span class=\"k\">,</span>","                                     e<span class=\"k\">.</span>ctrlKey<span class=\"k\">,</span> e<span class=\"k\">.</span>altKey<span class=\"k\">,</span> e<span class=\"k\">.</span>shiftKey<span class=\"k\">,</span> e<span class=\"k\">.</span>metaKey<span class=\"k\">,</span>","                                     e<span class=\"k\">.</span>keyCode<span class=\"k\">,</span> e<span class=\"k\">.</span>charCode<span class=\"k\">);</span>","                <span class=\"k\">}</span> <span class=\"k\">catch</span> <span class=\"k\">(</span>err<span class=\"k\">)</span> <span class=\"k\">{</span>","                    evt <span class=\"k\">=</span> document<span class=\"k\">.</span>createEvent<span class=\"k\">(</span><span class=\"s\">\"Events\"</span><span class=\"k\">);</span>","                    evt<span class=\"k\">.</span>initEvent<span class=\"k\">(</span>type<span class=\"k\">,</span> e<span class=\"k\">.</span>bubbles<span class=\"k\">,</span> e<span class=\"k\">.</span>cancelable<span class=\"k\">);</span>","                    $<span class=\"k\">.</span>extend<span class=\"k\">(</span>evt<span class=\"k\">,</span> <span class=\"k\">{</span> view<span class=\"k\">:</span> e<span class=\"k\">.</span>view<span class=\"k\">,</span>","                        ctrlKey<span class=\"k\">:</span> e<span class=\"k\">.</span>ctrlKey<span class=\"k\">,</span> altKey<span class=\"k\">:</span> e<span class=\"k\">.</span>altKey<span class=\"k\">,</span> shiftKey<span class=\"k\">:</span> e<span class=\"k\">.</span>shiftKey<span class=\"k\">,</span> metaKey<span class=\"k\">:</span> e<span class=\"k\">.</span>metaKey<span class=\"k\">,</span>","                        keyCode<span class=\"k\">:</span> e<span class=\"k\">.</span>keyCode<span class=\"k\">,</span> charCode<span class=\"k\">:</span> e<span class=\"k\">.</span>charCode","                    <span class=\"k\">}</span><span class=\"k\">);</span>","                <span class=\"k\">}</span>","            <span class=\"k\">}</span> <span class=\"k\">else</span> <span class=\"k\">if</span> <span class=\"k\">(</span>document<span class=\"k\">.</span>createEventObject<span class=\"k\">)</span> <span class=\"k\">{</span>","                evt <span class=\"k\">=</span> document<span class=\"k\">.</span>createEventObject<span class=\"k\">();</span>","                $<span class=\"k\">.</span>extend<span class=\"k\">(</span>evt<span class=\"k\">,</span> e<span class=\"k\">);</span>","            <span class=\"k\">}</span>","            <span class=\"k\">if</span> <span class=\"k\">(</span>$<span class=\"k\">.</span>browser<span class=\"k\">.</span>msie <span class=\"k\">||</span> $<span class=\"k\">.</span>browser<span class=\"k\">.</span>opera<span class=\"k\">)</span> <span class=\"k\">{</span>","                evt<span class=\"k\">.</span>keyCode <span class=\"k\">=</span> <span class=\"k\">(</span>e<span class=\"k\">.</span>charCode <span class=\"k\">&gt;</span> <span class=\"s\">0</span><span class=\"k\">)</span> <span class=\"k\">?</span> e<span class=\"k\">.</span>charCode <span class=\"k\">:</span> e<span class=\"k\">.</span>keyCode<span class=\"k\">;</span>","                evt<span class=\"k\">.</span>charCode <span class=\"k\">=</span> undefined<span class=\"k\">;</span>","            <span class=\"k\">}</span>","            <span class=\"k\">return</span> evt<span class=\"k\">;</span>","        <span class=\"k\">}</span><span class=\"k\">,</span>","","        dispatchEvent<span class=\"k\">:</span> <span class=\"k\">function</span> <span class=\"k\">(</span>el<span class=\"k\">,</span> type<span class=\"k\">,</span> evt<span class=\"k\">)</span> <span class=\"k\">{</span>","            <span class=\"k\">if</span> <span class=\"k\">(</span>el<span class=\"k\">.</span>dispatchEvent<span class=\"k\">)</span> <span class=\"k\">{</span>","                el<span class=\"k\">.</span>dispatchEvent<span class=\"k\">(</span>evt<span class=\"k\">);</span>","            <span class=\"k\">}</span> <span class=\"k\">else</span> <span class=\"k\">if</span> <span class=\"k\">(</span>el<span class=\"k\">.</span>fireEvent<span class=\"k\">)</span> <span class=\"k\">{</span>","                el<span class=\"k\">.</span>fireEvent<span class=\"k\">(</span><span class=\"s\">'on'</span> <span class=\"k\">+</span> type<span class=\"k\">,</span> evt<span class=\"k\">);</span>","            <span class=\"k\">}</span>","            <span class=\"k\">return</span> evt<span class=\"k\">;</span>","        <span class=\"k\">}</span><span class=\"k\">,</span>","","        drag<span class=\"k\">:</span> <span class=\"k\">function</span> <span class=\"k\">(</span>el<span class=\"k\">)</span> <span class=\"k\">{</span>","            <span class=\"k\">var</span> self <span class=\"k\">=</span> <span class=\"k\">this</span><span class=\"k\">,</span> center <span class=\"k\">=</span> <span class=\"k\">this</span><span class=\"k\">.</span>findCenter<span class=\"k\">(</span><span class=\"k\">this</span><span class=\"k\">.</span>target<span class=\"k\">),</span>","                options <span class=\"k\">=</span> <span class=\"k\">this</span><span class=\"k\">.</span>options<span class=\"k\">,</span> x <span class=\"k\">=</span> Math<span class=\"k\">.</span>floor<span class=\"k\">(</span>center<span class=\"k\">.</span>x<span class=\"k\">),</span> y <span class=\"k\">=</span> Math<span class=\"k\">.</span>floor<span class=\"k\">(</span>center<span class=\"k\">.</span>y<span class=\"k\">),</span>","                dx <span class=\"k\">=</span> options<span class=\"k\">.</span>dx <span class=\"k\">||</span> <span class=\"s\">0</span><span class=\"k\">,</span> dy <span class=\"k\">=</span> options<span class=\"k\">.</span>dy <span class=\"k\">||</span> <span class=\"s\">0</span><span class=\"k\">,</span> target <span class=\"k\">=</span> <span class=\"k\">this</span><span class=\"k\">.</span>target<span class=\"k\">;</span>","            <span class=\"k\">var</span> coord <span class=\"k\">=</span> <span class=\"k\">{</span> clientX<span class=\"k\">:</span> x<span class=\"k\">,</span> clientY<span class=\"k\">:</span> y <span class=\"k\">}</span><span class=\"k\">;</span>","            <span class=\"k\">this</span><span class=\"k\">.</span>simulateEvent<span class=\"k\">(</span>target<span class=\"k\">,</span> <span class=\"s\">\"mousedown\"</span><span class=\"k\">,</span> coord<span class=\"k\">);</span>","            coord <span class=\"k\">=</span> <span class=\"k\">{</span> clientX<span class=\"k\">:</span> x <span class=\"k\">+</span> <span class=\"s\">1</span><span class=\"k\">,</span> clientY<span class=\"k\">:</span> y <span class=\"k\">+</span> <span class=\"s\">1</span> <span class=\"k\">}</span><span class=\"k\">;</span>","            <span class=\"k\">this</span><span class=\"k\">.</span>simulateEvent<span class=\"k\">(</span>document<span class=\"k\">,</span> <span class=\"s\">\"mousemove\"</span><span class=\"k\">,</span> coord<span class=\"k\">);</span>","            coord <span class=\"k\">=</span> <span class=\"k\">{</span> clientX<span class=\"k\">:</span> x <span class=\"k\">+</span> dx<span class=\"k\">,</span> clientY<span class=\"k\">:</span> y <span class=\"k\">+</span> dy <span class=\"k\">}</span><span class=\"k\">;</span>","            <span class=\"k\">this</span><span class=\"k\">.</span>simulateEvent<span class=\"k\">(</span>document<span class=\"k\">,</span> <span class=\"s\">\"mousemove\"</span><span class=\"k\">,</span> coord<span class=\"k\">);</span>","            <span class=\"k\">this</span><span class=\"k\">.</span>simulateEvent<span class=\"k\">(</span>document<span class=\"k\">,</span> <span class=\"s\">\"mousemove\"</span><span class=\"k\">,</span> coord<span class=\"k\">);</span>","            <span class=\"k\">this</span><span class=\"k\">.</span>simulateEvent<span class=\"k\">(</span>target<span class=\"k\">,</span> <span class=\"s\">\"mouseup\"</span><span class=\"k\">,</span> coord<span class=\"k\">);</span>","        <span class=\"k\">}</span><span class=\"k\">,</span>","        findCenter<span class=\"k\">:</span> <span class=\"k\">function</span> <span class=\"k\">(</span>el<span class=\"k\">)</span> <span class=\"k\">{</span>","            <span class=\"k\">var</span> el <span class=\"k\">=</span> $<span class=\"k\">(</span><span class=\"k\">this</span><span class=\"k\">.</span>target<span class=\"k\">),</span> o <span class=\"k\">=</span> el<span class=\"k\">.</span>offset<span class=\"k\">();</span>","            <span class=\"k\">return</span> <span class=\"k\">{</span>","                x<span class=\"k\">:</span> o<span class=\"k\">.</span>left <span class=\"k\">+</span> el<span class=\"k\">.</span>outerWidth<span class=\"k\">()</span> <span class=\"k\">/</span> <span class=\"s\">2</span><span class=\"k\">,</span>","                y<span class=\"k\">:</span> o<span class=\"k\">.</span>top <span class=\"k\">+</span> el<span class=\"k\">.</span>outerHeight<span class=\"k\">()</span> <span class=\"k\">/</span> <span class=\"s\">2</span>","            <span class=\"k\">}</span><span class=\"k\">;</span>","        <span class=\"k\">}</span>","    <span class=\"k\">}</span><span class=\"k\">);</span>","","    $<span class=\"k\">.</span>extend<span class=\"k\">(</span>$<span class=\"k\">.</span>simulate<span class=\"k\">,</span> <span class=\"k\">{</span>","        defaults<span class=\"k\">:</span> <span class=\"k\">{</span>","            speed<span class=\"k\">:</span> <span class=\"s\">'sync'</span>","        <span class=\"k\">}</span><span class=\"k\">,</span>","        VK_TAB<span class=\"k\">:</span> <span class=\"s\">9</span><span class=\"k\">,</span>","        VK_ENTER<span class=\"k\">:</span> <span class=\"s\">13</span><span class=\"k\">,</span>","        VK_ESC<span class=\"k\">:</span> <span class=\"s\">27</span><span class=\"k\">,</span>","        VK_PGUP<span class=\"k\">:</span> <span class=\"s\">33</span><span class=\"k\">,</span>","        VK_PGDN<span class=\"k\">:</span> <span class=\"s\">34</span><span class=\"k\">,</span>","        VK_END<span class=\"k\">:</span> <span class=\"s\">35</span><span class=\"k\">,</span>","        VK_HOME<span class=\"k\">:</span> <span class=\"s\">36</span><span class=\"k\">,</span>","        VK_LEFT<span class=\"k\">:</span> <span class=\"s\">37</span><span class=\"k\">,</span>","        VK_UP<span class=\"k\">:</span> <span class=\"s\">38</span><span class=\"k\">,</span>","        VK_RIGHT<span class=\"k\">:</span> <span class=\"s\">39</span><span class=\"k\">,</span>","        VK_DOWN<span class=\"k\">:</span> <span class=\"s\">40</span>","    <span class=\"k\">}</span><span class=\"k\">);</span>","","<span class=\"k\">}</span><span class=\"k\">)(</span>jQuery<span class=\"k\">);</span>"];
_$jscoverage['jquery.simulate.js'][13]++;
(function ($) {
  _$jscoverage['jquery.simulate.js'][15]++;
  $.fn.extend({simulate: (function (type, options) {
  _$jscoverage['jquery.simulate.js'][17]++;
  return this.each((function () {
  _$jscoverage['jquery.simulate.js'][18]++;
  var opt = $.extend({}, $.simulate.defaults, (options || {}));
  _$jscoverage['jquery.simulate.js'][19]++;
  new ($.simulate)(this, type, opt);
}));
})});
  _$jscoverage['jquery.simulate.js'][24]++;
  $.simulate = (function (el, type, options) {
  _$jscoverage['jquery.simulate.js'][25]++;
  this.target = el;
  _$jscoverage['jquery.simulate.js'][26]++;
  this.options = options;
  _$jscoverage['jquery.simulate.js'][28]++;
  if (/^drag$/.test(type)) {
    _$jscoverage['jquery.simulate.js'][29]++;
    this[type].apply(this, [this.target, options]);
  }
  else {
    _$jscoverage['jquery.simulate.js'][31]++;
    this.simulateEvent(el, type, options);
  }
});
  _$jscoverage['jquery.simulate.js'][35]++;
  $.extend($.simulate.prototype, {simulateEvent: (function (el, type, options) {
  _$jscoverage['jquery.simulate.js'][37]++;
  var evt = this.createEvent(type, options);
  _$jscoverage['jquery.simulate.js'][38]++;
  this.dispatchEvent(el, type, evt, options);
  _$jscoverage['jquery.simulate.js'][39]++;
  return evt;
}), createEvent: (function (type, options) {
  _$jscoverage['jquery.simulate.js'][42]++;
  if (/^mouse(over|out|down|up|move)|(dbl)?click$/.test(type)) {
    _$jscoverage['jquery.simulate.js'][43]++;
    return this.mouseEvent(type, options);
  }
  else {
    _$jscoverage['jquery.simulate.js'][44]++;
    if (/^key(up|down|press)$/.test(type)) {
      _$jscoverage['jquery.simulate.js'][45]++;
      return this.keyboardEvent(type, options);
    }
  }
}), mouseEvent: (function (type, options) {
  _$jscoverage['jquery.simulate.js'][49]++;
  var evt;
  _$jscoverage['jquery.simulate.js'][50]++;
  var e = $.extend({which: 1, bubbles: true, cancelable: (type != "mousemove"), view: window, detail: 0, screenX: 0, screenY: 0, clientX: 0, clientY: 0, ctrlKey: false, altKey: false, shiftKey: false, metaKey: false, button: 0, relatedTarget: undefined}, options);
  _$jscoverage['jquery.simulate.js'][58]++;
  var relatedTarget = $(e.relatedTarget)[0];
  _$jscoverage['jquery.simulate.js'][60]++;
  if ($.isFunction(document.createEvent)) {
    _$jscoverage['jquery.simulate.js'][61]++;
    evt = document.createEvent("MouseEvents");
    _$jscoverage['jquery.simulate.js'][62]++;
    evt.initMouseEvent(type, e.bubbles, e.cancelable, e.view, e.detail, e.screenX, e.screenY, e.clientX, e.clientY, e.ctrlKey, e.altKey, e.shiftKey, e.metaKey, e.button, (e.relatedTarget || document.body.parentNode));
  }
  else {
    _$jscoverage['jquery.simulate.js'][66]++;
    if (document.createEventObject) {
      _$jscoverage['jquery.simulate.js'][67]++;
      evt = document.createEventObject();
      _$jscoverage['jquery.simulate.js'][68]++;
      $.extend(evt, e);
      _$jscoverage['jquery.simulate.js'][69]++;
      evt.button = ({0: 1, 1: 4, 2: 2}[evt.button] || evt.button);
    }
  }
  _$jscoverage['jquery.simulate.js'][71]++;
  return evt;
}), keyboardEvent: (function (type, options) {
  _$jscoverage['jquery.simulate.js'][74]++;
  var evt;
  _$jscoverage['jquery.simulate.js'][76]++;
  var e = $.extend({bubbles: true, cancelable: true, view: window, ctrlKey: false, altKey: false, shiftKey: false, metaKey: false, keyCode: 0, charCode: 0}, options);
  _$jscoverage['jquery.simulate.js'][81]++;
  if ($.isFunction(document.createEvent)) {
    _$jscoverage['jquery.simulate.js'][82]++;
    try {
      _$jscoverage['jquery.simulate.js'][83]++;
      evt = document.createEvent("KeyEvents");
      _$jscoverage['jquery.simulate.js'][84]++;
      evt.initKeyEvent(type, e.bubbles, e.cancelable, e.view, e.ctrlKey, e.altKey, e.shiftKey, e.metaKey, e.keyCode, e.charCode);
    }
    catch (err) {
      _$jscoverage['jquery.simulate.js'][88]++;
      evt = document.createEvent("Events");
      _$jscoverage['jquery.simulate.js'][89]++;
      evt.initEvent(type, e.bubbles, e.cancelable);
      _$jscoverage['jquery.simulate.js'][90]++;
      $.extend(evt, {view: e.view, ctrlKey: e.ctrlKey, altKey: e.altKey, shiftKey: e.shiftKey, metaKey: e.metaKey, keyCode: e.keyCode, charCode: e.charCode});
    }
  }
  else {
    _$jscoverage['jquery.simulate.js'][95]++;
    if (document.createEventObject) {
      _$jscoverage['jquery.simulate.js'][96]++;
      evt = document.createEventObject();
      _$jscoverage['jquery.simulate.js'][97]++;
      $.extend(evt, e);
    }
  }
  _$jscoverage['jquery.simulate.js'][99]++;
  if (($.browser.msie || $.browser.opera)) {
    _$jscoverage['jquery.simulate.js'][100]++;
    evt.keyCode = ((e.charCode > 0)? e.charCode: e.keyCode);
    _$jscoverage['jquery.simulate.js'][101]++;
    evt.charCode = undefined;
  }
  _$jscoverage['jquery.simulate.js'][103]++;
  return evt;
}), dispatchEvent: (function (el, type, evt) {
  _$jscoverage['jquery.simulate.js'][107]++;
  if (el.dispatchEvent) {
    _$jscoverage['jquery.simulate.js'][108]++;
    el.dispatchEvent(evt);
  }
  else {
    _$jscoverage['jquery.simulate.js'][109]++;
    if (el.fireEvent) {
      _$jscoverage['jquery.simulate.js'][110]++;
      el.fireEvent(("on" + type), evt);
    }
  }
  _$jscoverage['jquery.simulate.js'][112]++;
  return evt;
}), drag: (function (el) {
  _$jscoverage['jquery.simulate.js'][116]++;
  var self = this, center = this.findCenter(this.target), options = this.options, x = Math.floor(center.x), y = Math.floor(center.y), dx = (options.dx || 0), dy = (options.dy || 0), target = this.target;
  _$jscoverage['jquery.simulate.js'][119]++;
  var coord = {clientX: x, clientY: y};
  _$jscoverage['jquery.simulate.js'][120]++;
  this.simulateEvent(target, "mousedown", coord);
  _$jscoverage['jquery.simulate.js'][121]++;
  coord = {clientX: (x + 1), clientY: (y + 1)};
  _$jscoverage['jquery.simulate.js'][122]++;
  this.simulateEvent(document, "mousemove", coord);
  _$jscoverage['jquery.simulate.js'][123]++;
  coord = {clientX: (x + dx), clientY: (y + dy)};
  _$jscoverage['jquery.simulate.js'][124]++;
  this.simulateEvent(document, "mousemove", coord);
  _$jscoverage['jquery.simulate.js'][125]++;
  this.simulateEvent(document, "mousemove", coord);
  _$jscoverage['jquery.simulate.js'][126]++;
  this.simulateEvent(target, "mouseup", coord);
}), findCenter: (function (el) {
  _$jscoverage['jquery.simulate.js'][129]++;
  var el = $(this.target), o = el.offset();
  _$jscoverage['jquery.simulate.js'][130]++;
  return ({x: (o.left + (el.outerWidth() / 2)), y: (o.top + (el.outerHeight() / 2))});
})});
  _$jscoverage['jquery.simulate.js'][137]++;
  $.extend($.simulate, {defaults: {speed: "sync"}, VK_TAB: 9, VK_ENTER: 13, VK_ESC: 27, VK_PGUP: 33, VK_PGDN: 34, VK_END: 35, VK_HOME: 36, VK_LEFT: 37, VK_UP: 38, VK_RIGHT: 39, VK_DOWN: 40});
})(jQuery);