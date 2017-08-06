<!DOCTYPE HTML>
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en"><head> 
	<meta http-equiv="content-type" content="application/xhtml+xml; charset=utf-8" />
	<title>Converting SVG Path to Polygon</title>
	<style type="text/css" media="screen">
		body { background:#eee; margin:1em }
		h1 { font-size:110%; margin:0; border-bottom:1px solid #ccc }
		p { margin-bottom:0.2em;}
		svg { display:block; border:1px solid #ccc; margin:0 -1px 3em -1px; background:#fff; height:150px; width:100%; margin }
		svg.labeled { height:175px }
		svg path, svg polygon { stroke:#000; stroke-width:1px; stroke-linecap:round; stroke-linejoin:miter; stroke-miterlimit:10; fill:rgb(255,0,0) }
		svg text { stroke:none; alignment-baseline:text-before-edge; fill:#666; }
		svg use { stroke:#000; stroke-width:1px; opacity:0.3; stroke-dasharray:1,3; fill:#999; }
		svg use.dot { stroke:none; opacity:1.0 }
		#footer { color:#666; font-size:85%; font-style:italic; margin-top:3em }
	</style>
</head>
<body>

<form runat="server">


<p>Here we see an SVG <code>&lt;path&gt;</code> element using Bézier curves:</p>

<svg viewBox="0 0 100 100" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" version="1.1" baseProfile="full">
	<defs><circle id="dot" cx="0" cy="0" r="2" fill="#06c" opacity="0.9" stroke="none"/></defs>
	<path id="bat" d="M3.322,53.401C5.515,4.08,14.59,37.193,40.366,23.5C66.14,9.807,15.912-1.583,83.616,13
		c16.25,3.5,13.271,28.521,3,18.25C62.452,7.086,70.866,38,96.757,62.262C61.316,41.5,44.896,39.755,62.616,83.25
		c-65.25-8-13.382-33.876-25.464-7.295S1.711,89.647,3.322,53.401z"/>
</svg>
<script type="text/javascript"><![CDATA[
    var bat = document.getElementById('bat');
    var dot = document.getElementById('dot');
    function createOn(root, name, attrs, text) {
        var doc = root.ownerDocument;
        var svg = root;
        while (svg.tagName != 'svg') svg = svg.parentNode;
        var svgNS = svg.getAttribute('xmlns');
        var el = doc.createElementNS(svgNS, name);
        for (var attr in attrs) {
            if (attrs.hasOwnProperty(attr)) {
                var parts = attr.split(':');
                if (parts[1]) el.setAttributeNS(svg.getAttribute('xmlns:' + parts[0]), parts[1], attrs[attr]);
                else el.setAttributeNS(null, attr, attrs[attr]);
            }
        }
        if (text) el.appendChild(document.createTextNode(text));
        return root.appendChild(el);
    }	
]]></script>

<p>We can very simply convert this to a <code>&lt;polygon&gt;</code> by sampling the path at regular intervals, using <a href="http://objjob.phrogz.net/svg/method/513"><code>getPointAtLength()</code></a>:</p>
<svg id="sampled" class="labeled" viewBox="0 0 100 125" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" version="1.1" baseProfile="full"></svg>
<script type="text/javascript"><![CDATA[
    function polygonSampledFromPath(path, samples) {
        var doc = path.ownerDocument;
        var poly = doc.createElementNS('http://www.w3.org/2000/svg', 'polygon');

        var points = [];
        var len = path.getTotalLength();
        var step = step = len / samples;
        for (var i = 0; i <= len; i += step) {
            var p = path.getPointAtLength(i);
            points.push(p.x + ',' + p.y);
        }
        poly.setAttribute('points', points.join(' '));
        return poly;
    }

    function createSamples(svg, func) {
        var samples = [
			{ count: 9, offset: -200 }, { count: 15, offset: -100 },
			{ count: 25, offset: 0 },
			{ count: 50, offset: 100 }, { count: 75, offset: 200 }
		];
        for (var i = samples.length - 1; i >= 0; --i) {
            var sample = samples[i];
            var g = createOn(svg, 'g', { transform: "translate(" + sample.offset + ",0)" });
            createOn(g, 'use', { "xlink:href": "#bat" });
            var poly = g.appendChild(func(bat, sample.count));
            createOn(g, 'text', { y: 100 }, sample.count + " samples");
            for (var j = poly.points.numberOfItems - 1; j >= 0; --j) {
                var pt = poly.points.getItem(j);
                createOn(g, 'use', { "xlink:href": "#dot", x: pt.x, y: pt.y, "class": "dot" });
            }
        }
    }
    createSamples(document.getElementById('sampled'), polygonSampledFromPath);
]]></script>

<p>As seen above, sampling may miss areas important to the shape of the object. The sharp points on the object, for example, are missed by the 9, 15, and 25 sample versions above. We can improve our result by ensuring that control points from the original path are included in the polygon:</p>
<svg id="controlled" class="labeled" viewBox="0 0 100 125" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" version="1.1" baseProfile="full"></svg>
<script type="text/javascript"><![CDATA[
    function pathToPolygon(path, samples) {
        if (!samples) samples = 0;
        var doc = path.ownerDocument;
        var poly = doc.createElementNS('http://www.w3.org/2000/svg', 'polygon');

        // Put all path segments in a queue
        for (var segs = [], s = path.pathSegList, i = s.numberOfItems - 1; i >= 0; --i) segs[i] = s.getItem(i);
        var segments = segs.concat();

        var seg, lastSeg, points = [], x, y;
        var addSegmentPoint = function (s) {
            if (s.pathSegType == SVGPathSeg.PATHSEG_CLOSEPATH) {

            } else {
                if (s.pathSegType % 2 == 1 && s.pathSegType > 1) {
                    // All odd-numbered path types are relative, except PATHSEG_CLOSEPATH (1)
                    x += s.x; y += s.y;
                } else {
                    x = s.x; y = s.y;
                }
                var lastPoint = points[points.length - 1];
                if (!lastPoint || x != lastPoint[0] || y != lastPoint[1]) points.push([x, y]);
            }
        };
        for (var d = 0, len = path.getTotalLength(), step = len / samples; d <= len; d += step) {
            var seg = segments[path.getPathSegAtLength(d)];
            var pt = path.getPointAtLength(d);
            if (seg != lastSeg) {
                lastSeg = seg;
                while (segs.length && segs[0] != seg) addSegmentPoint(segs.shift());
            }
            var lastPoint = points[points.length - 1];
            if (!lastPoint || pt.x != lastPoint[0] || pt.y != lastPoint[1]) points.push([pt.x, pt.y]);
        }
        for (var i = 0, len = segs.length; i < len; ++i) addSegmentPoint(segs[i]);
        for (var i = 0, len = points.length; i < len; ++i) points[i] = points[i].join(',');
        poly.setAttribute('points', points.join(' '));
        return poly;
    }
    createSamples(document.getElementById('controlled'), pathToPolygon);
]]>
</script>




</form>


</body>

</html>