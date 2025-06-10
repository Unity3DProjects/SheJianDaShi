var r = 0,
    n = "",
    o = 8;

function i(e, t) {
    e[t >> 5] |= 128 << t % 32, e[14 + (t + 64 >>> 9 << 4)] = t;
    for (var r = 1732584193, n = -271733879, o = -1732584194, i = 271733878, a = 0; a < e.length; a += 16) {
        var c = r,
            h = n,
            p = o,
            w = i;
        r = s(r, n, o, i, e[a + 0], 7, -680876936), i = s(i, r, n, o, e[a + 1], 12, -389564586), o = s(o, i, r, n, e[a + 2], 17, 606105819), n = s(n, o, i, r, e[a + 3], 22, -1044525330), r = s(r, n, o, i, e[a + 4], 7, -176418897), i = s(i, r, n, o, e[a + 5], 12, 1200080426), o = s(o, i, r, n, e[a + 6], 17, -1473231341), n = s(n, o, i, r, e[a + 7], 22, -45705983), r = s(r, n, o, i, e[a + 8], 7, 1770035416), i = s(i, r, n, o, e[a + 9], 12, -1958414417), o = s(o, i, r, n, e[a + 10], 17, -42063), n = s(n, o, i, r, e[a + 11], 22, -1990404162), r = s(r, n, o, i, e[a + 12], 7, 1804603682), i = s(i, r, n, o, e[a + 13], 12, -40341101), o = s(o, i, r, n, e[a + 14], 17, -1502002290), r = d(r, n = s(n, o, i, r, e[a + 15], 22, 1236535329), o, i, e[a + 1], 5, -165796510), i = d(i, r, n, o, e[a + 6], 9, -1069501632), o = d(o, i, r, n, e[a + 11], 14, 643717713), n = d(n, o, i, r, e[a + 0], 20, -373897302), r = d(r, n, o, i, e[a + 5], 5, -701558691), i = d(i, r, n, o, e[a + 10], 9, 38016083), o = d(o, i, r, n, e[a + 15], 14, -660478335), n = d(n, o, i, r, e[a + 4], 20, -405537848), r = d(r, n, o, i, e[a + 9], 5, 568446438), i = d(i, r, n, o, e[a + 14], 9, -1019803690), o = d(o, i, r, n, e[a + 3], 14, -187363961), n = d(n, o, i, r, e[a + 8], 20, 1163531501), r = d(r, n, o, i, e[a + 13], 5, -1444681467), i = d(i, r, n, o, e[a + 2], 9, -51403784), o = d(o, i, r, n, e[a + 7], 14, 1735328473), r = l(r, n = d(n, o, i, r, e[a + 12], 20, -1926607734), o, i, e[a + 5], 4, -378558), i = l(i, r, n, o, e[a + 8], 11, -2022574463), o = l(o, i, r, n, e[a + 11], 16, 1839030562), n = l(n, o, i, r, e[a + 14], 23, -35309556), r = l(r, n, o, i, e[a + 1], 4, -1530992060), i = l(i, r, n, o, e[a + 4], 11, 1272893353), o = l(o, i, r, n, e[a + 7], 16, -155497632), n = l(n, o, i, r, e[a + 10], 23, -1094730640), r = l(r, n, o, i, e[a + 13], 4, 681279174), i = l(i, r, n, o, e[a + 0], 11, -358537222), o = l(o, i, r, n, e[a + 3], 16, -722521979), n = l(n, o, i, r, e[a + 6], 23, 76029189), r = l(r, n, o, i, e[a + 9], 4, -640364487), i = l(i, r, n, o, e[a + 12], 11, -421815835), o = l(o, i, r, n, e[a + 15], 16, 530742520), r = u(r, n = l(n, o, i, r, e[a + 2], 23, -995338651), o, i, e[a + 0], 6, -198630844), i = u(i, r, n, o, e[a + 7], 10, 1126891415), o = u(o, i, r, n, e[a + 14], 15, -1416354905), n = u(n, o, i, r, e[a + 5], 21, -57434055), r = u(r, n, o, i, e[a + 12], 6, 1700485571), i = u(i, r, n, o, e[a + 3], 10, -1894986606), o = u(o, i, r, n, e[a + 10], 15, -1051523), n = u(n, o, i, r, e[a + 1], 21, -2054922799), r = u(r, n, o, i, e[a + 8], 6, 1873313359), i = u(i, r, n, o, e[a + 15], 10, -30611744), o = u(o, i, r, n, e[a + 6], 15, -1560198380), n = u(n, o, i, r, e[a + 13], 21, 1309151649), r = u(r, n, o, i, e[a + 4], 6, -145523070), i = u(i, r, n, o, e[a + 11], 10, -1120210379), o = u(o, i, r, n, e[a + 2], 15, 718787259), n = u(n, o, i, r, e[a + 9], 21, -343485551), r = f(r, c), n = f(n, h), o = f(o, p), i = f(i, w)
    }
    return Array(r, n, o, i)
}

function a(e, t, r, n, o, i) {
    return f((a = f(f(t, e), f(n, i))) << (s = o) | a >>> 32 - s, r);
    var a, s
}

function s(e, t, r, n, o, i, s) {
    return a(t & r | ~t & n, e, t, o, i, s)
}

function d(e, t, r, n, o, i, s) {
    return a(t & n | r & ~n, e, t, o, i, s)
}

function l(e, t, r, n, o, i, s) {
    return a(t ^ r ^ n, e, t, o, i, s)
}

function u(e, t, r, n, o, i, s) {
    return a(r ^ (t | ~n), e, t, o, i, s)
}

function c(e, t) {
    var r = h(e);
    r.length > 16 && (r = i(r, e.length * o));
    for (var n = Array(16), a = Array(16), s = 0; s < 16; s++) n[s] = 909522486 ^ r[s], a[s] = 1549556828 ^ r[s];
    var d = i(n.concat(h(t)), 512 + t.length * o);
    return i(a.concat(d), 640)
}

function f(e, t) {
    var r = (65535 & e) + (65535 & t);
    return (e >> 16) + (t >> 16) + (r >> 16) << 16 | 65535 & r
}

function h(e) {
    for (var t = Array(), r = (1 << o) - 1, n = 0; n < e.length * o; n += o) t[n >> 5] |= (e.charCodeAt(n / o) & r) << n % 32;
    return t
}

function p(e) {
    for (var t = "", r = (1 << o) - 1, n = 0; n < 32 * e.length; n += o) t += String.fromCharCode(e[n >> 5] >>> n % 32 & r);
    return t
}

function w(e) {
    for (var t = r ? "0123456789ABCDEF" : "0123456789abcdef", n = "", o = 0; o < 4 * e.length; o++) n += t.charAt(e[o >> 2] >> o % 4 * 8 + 4 & 15) + t.charAt(e[o >> 2] >> o % 4 * 8 & 15);
    return n
}

function m(e) {
    for (var t = "", r = 0; r < 4 * e.length; r += 3)
        for (var o = (e[r >> 2] >> r % 4 * 8 & 255) << 16 | (e[r + 1 >> 2] >> (r + 1) % 4 * 8 & 255) << 8 | e[r + 2 >> 2] >> (r + 2) % 4 * 8 & 255, i = 0; i < 4; i++) 8 * r + 6 * i > 32 * e.length ? t += n : t += "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/".charAt(o >> 6 * (3 - i) & 63);
    return t
}

function hex_md5(e) {
    return w(i(h(e), e.length * o))
}
function b64_md5(e) {
    return m(i(h(e), e.length * o))
}
function str_md5(e) {
    return p(i(h(e), e.length * o))
}
function hex_hmac_md5(e, t) {
    return w(c(e, t))
}
function b64_hmac_md5(e, t) {
    return m(c(e, t))
}
function str_hmac_md5(e, t) {
    return p(c(e, t))
}

module.exports = {
    hex_md5
}
