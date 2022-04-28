!(function (e, t) {
    "object" == typeof exports && "object" == typeof module ? (module.exports = t()) : "function" == typeof define && define.amd ? define([], t) : "object" == typeof exports ? (exports.DailyIframe = t()) : (e.DailyIframe = t());
})(this, function () {
    return (function (e) {
        var t = {};
        function r(n) {
            if (t[n]) return t[n].exports;
            var i = (t[n] = { i: n, l: !1, exports: {} });
            return e[n].call(i.exports, i, i.exports, r), (i.l = !0), i.exports;
        }
        return (
            (r.m = e),
            (r.c = t),
            (r.d = function (e, t, n) {
                r.o(e, t) || Object.defineProperty(e, t, { enumerable: !0, get: n });
            }),
            (r.r = function (e) {
                "undefined" != typeof Symbol && Symbol.toStringTag && Object.defineProperty(e, Symbol.toStringTag, { value: "Module" }), Object.defineProperty(e, "__esModule", { value: !0 });
            }),
            (r.t = function (e, t) {
                if ((1 & t && (e = r(e)), 8 & t)) return e;
                if (4 & t && "object" == typeof e && e && e.__esModule) return e;
                var n = Object.create(null);
                if ((r.r(n), Object.defineProperty(n, "default", { enumerable: !0, value: e }), 2 & t && "string" != typeof e))
                    for (var i in e)
                        r.d(
                            n,
                            i,
                            function (t) {
                                return e[t];
                            }.bind(null, i)
                        );
                return n;
            }),
            (r.n = function (e) {
                var t =
                    e && e.__esModule
                        ? function () {
                            return e.default;
                        }
                        : function () {
                            return e;
                        };
                return r.d(t, "a", t), t;
            }),
            (r.o = function (e, t) {
                return Object.prototype.hasOwnProperty.call(e, t);
            }),
            (r.p = ""),
            r((r.s = 63))
        );
    })([
        function (e, t, r) {
            e.exports = r(64);
        },
        function (e, t) {
            function r(e, t, r, n, i, a, o) {
                try {
                    var s = e[a](o),
                        c = s.value;
                } catch (e) {
                    return void r(e);
                }
                s.done ? t(c) : Promise.resolve(c).then(n, i);
            }
            e.exports = function (e) {
                return function () {
                    var t = this,
                        n = arguments;
                    return new Promise(function (i, a) {
                        var o = e.apply(t, n);
                        function s(e) {
                            r(o, i, a, s, c, "next", e);
                        }
                        function c(e) {
                            r(o, i, a, s, c, "throw", e);
                        }
                        s(void 0);
                    });
                };
            };
        },
        function (e, t, r) {
            "use strict";
            r.d(t, "b", function () {
                return a;
            }),
                r.d(t, "a", function () {
                    return o;
                });
            var n = r(36),
                i = r.n(n);
            function a() {
                return "undefined" != typeof navigator && navigator.product && "ReactNative" === navigator.product;
            }
            function o() {
                if (a()) return { supported: !0, mobile: !0, name: "React Native", version: null, supportsScreenShare: !1, supportsSfu: !0 };
                var e = i.a.getParser(window.navigator.userAgent),
                    t = e.getBrowser(),
                    r = i.a.parse(window.navigator.userAgent),
                    n = e.satisfies({
                        electron: ">=6",
                        chromium: ">=61",
                        chrome: ">=61",
                        firefox: ">=63",
                        opera: ">=61",
                        safari: ">=12",
                        edge: ">=18",
                        iOS: { chromium: "<0", chrome: "<0", firefox: "<0", opera: "<0", safari: ">=12", edge: "<0" },
                    }),
                    o = !!(
                        n &&
                        navigator &&
                        navigator.mediaDevices &&
                        navigator.mediaDevices.getDisplayMedia &&
                        (function (e) {
                            return e.satisfies({ electron: ">=6", chromium: ">=75", chrome: ">=75", firefox: ">=67", opera: ">=61", safari: ">=13.0.1", edge: ">=79" });
                        })(e)
                    ),
                    s = !(!n || e.satisfies({ edge: "<=18" }));
                return { supported: n, mobile: "mobile" === r.platform.type, name: t.name, version: t.version, supportsScreenShare: o, supportsSfu: s };
            }
        },
        function (e, t) {
            e.exports = function (e, t) {
                if (!(e instanceof t)) throw new TypeError("Cannot call a class as a function");
            };
        },
        function (e, t) {
            function r(e, t) {
                for (var r = 0; r < t.length; r++) {
                    var n = t[r];
                    (n.enumerable = n.enumerable || !1), (n.configurable = !0), "value" in n && (n.writable = !0), Object.defineProperty(e, n.key, n);
                }
            }
            e.exports = function (e, t, n) {
                return t && r(e.prototype, t), n && r(e, n), e;
            };
        },
        function (e, t, r) {
            "use strict";
            r.d(t, "b", function () {
                return i;
            }),
                r.d(t, "a", function () {
                    return a;
                });
            var n = r(2);
            function i() {
                throw new Error("Method must be implemented in subclass");
            }
            function a(e) {
                var t = e ? new URL(e).origin : null;
                return !t || t.match(/https:\/\/[^.]+\.daily\.co/)
                    ? Object(n.a)().supportsSfu
                        ? "https://c.daily.co/static/call-machine-object-bundle.js"
                        : "https://c.daily.co/static/call-machine-object-nosfu-bundle.js"
                    : (t || (console.warn("No baseUrl provided for call object bundle. Defaulting to production CDN..."), (t = "https://c.daily.co")),
                        Object(n.a)().supportsSfu ? "".concat(t, "/static/call-machine-object-bundle.js") : "".concat(t, "/static/call-machine-object-nosfu-bundle.js"));
            }
        },
        function (e, t) {
            var r = Array.isArray;
            e.exports = r;
        },
        function (e, t, r) {
            var n = r(40),
                i = "object" == typeof self && self && self.Object === Object && self,
                a = n || i || Function("return this")();
            e.exports = a;
        },
        function (e, t) {
            function r(t) {
                return (
                    (e.exports = r = Object.setPrototypeOf
                        ? Object.getPrototypeOf
                        : function (e) {
                            return e.__proto__ || Object.getPrototypeOf(e);
                        }),
                    r(t)
                );
            }
            e.exports = r;
        },
        function (e, t, r) {
            var n = r(16),
                i = r(17);
            e.exports = function (e, t) {
                return !t || ("object" !== n(t) && "function" != typeof t) ? i(e) : t;
            };
        },
        function (e, t, r) {
            var n = r(28);
            e.exports = function (e, t) {
                if ("function" != typeof t && null !== t) throw new TypeError("Super expression must either be null or a function");
                (e.prototype = Object.create(t && t.prototype, { constructor: { value: e, writable: !0, configurable: !0 } })), t && n(e, t);
            };
        },
        function (e, t, r) {
            "use strict";
            var n,
                i = "object" == typeof Reflect ? Reflect : null,
                a =
                    i && "function" == typeof i.apply
                        ? i.apply
                        : function (e, t, r) {
                            return Function.prototype.apply.call(e, t, r);
                        };
            n =
                i && "function" == typeof i.ownKeys
                    ? i.ownKeys
                    : Object.getOwnPropertySymbols
                        ? function (e) {
                            return Object.getOwnPropertyNames(e).concat(Object.getOwnPropertySymbols(e));
                        }
                        : function (e) {
                            return Object.getOwnPropertyNames(e);
                        };
            var o =
                Number.isNaN ||
                function (e) {
                    return e != e;
                };
            function s() {
                s.init.call(this);
            }
            (e.exports = s), (s.EventEmitter = s), (s.prototype._events = void 0), (s.prototype._eventsCount = 0), (s.prototype._maxListeners = void 0);
            var c = 10;
            function u(e) {
                if ("function" != typeof e) throw new TypeError('The "listener" argument must be of type Function. Received type ' + typeof e);
            }
            function l(e) {
                return void 0 === e._maxListeners ? s.defaultMaxListeners : e._maxListeners;
            }
            function d(e, t, r, n) {
                var i, a, o, s;
                if (
                    (u(r),
                        void 0 === (a = e._events) ? ((a = e._events = Object.create(null)), (e._eventsCount = 0)) : (void 0 !== a.newListener && (e.emit("newListener", t, r.listener ? r.listener : r), (a = e._events)), (o = a[t])),
                        void 0 === o)
                )
                    (o = a[t] = r), ++e._eventsCount;
                else if (("function" == typeof o ? (o = a[t] = n ? [r, o] : [o, r]) : n ? o.unshift(r) : o.push(r), (i = l(e)) > 0 && o.length > i && !o.warned)) {
                    o.warned = !0;
                    var c = new Error("Possible EventEmitter memory leak detected. " + o.length + " " + String(t) + " listeners added. Use emitter.setMaxListeners() to increase limit");
                    (c.name = "MaxListenersExceededWarning"), (c.emitter = e), (c.type = t), (c.count = o.length), (s = c), console && console.warn && console.warn(s);
                }
                return e;
            }
            function f() {
                if (!this.fired) return this.target.removeListener(this.type, this.wrapFn), (this.fired = !0), 0 === arguments.length ? this.listener.call(this.target) : this.listener.apply(this.target, arguments);
            }
            function p(e, t, r) {
                var n = { fired: !1, wrapFn: void 0, target: e, type: t, listener: r },
                    i = f.bind(n);
                return (i.listener = r), (n.wrapFn = i), i;
            }
            function h(e, t, r) {
                var n = e._events;
                if (void 0 === n) return [];
                var i = n[t];
                return void 0 === i
                    ? []
                    : "function" == typeof i
                        ? r
                            ? [i.listener || i]
                            : [i]
                        : r
                            ? (function (e) {
                                for (var t = new Array(e.length), r = 0; r < t.length; ++r) t[r] = e[r].listener || e[r];
                                return t;
                            })(i)
                            : m(i, i.length);
            }
            function v(e) {
                var t = this._events;
                if (void 0 !== t) {
                    var r = t[e];
                    if ("function" == typeof r) return 1;
                    if (void 0 !== r) return r.length;
                }
                return 0;
            }
            function m(e, t) {
                for (var r = new Array(t), n = 0; n < t; ++n) r[n] = e[n];
                return r;
            }
            Object.defineProperty(s, "defaultMaxListeners", {
                enumerable: !0,
                get: function () {
                    return c;
                },
                set: function (e) {
                    if ("number" != typeof e || e < 0 || o(e)) throw new RangeError('The value of "defaultMaxListeners" is out of range. It must be a non-negative number. Received ' + e + ".");
                    c = e;
                },
            }),
                (s.init = function () {
                    (void 0 !== this._events && this._events !== Object.getPrototypeOf(this)._events) || ((this._events = Object.create(null)), (this._eventsCount = 0)), (this._maxListeners = this._maxListeners || void 0);
                }),
                (s.prototype.setMaxListeners = function (e) {
                    if ("number" != typeof e || e < 0 || o(e)) throw new RangeError('The value of "n" is out of range. It must be a non-negative number. Received ' + e + ".");
                    return (this._maxListeners = e), this;
                }),
                (s.prototype.getMaxListeners = function () {
                    return l(this);
                }),
                (s.prototype.emit = function (e) {
                    for (var t = [], r = 1; r < arguments.length; r++) t.push(arguments[r]);
                    var n = "error" === e,
                        i = this._events;
                    if (void 0 !== i) n = n && void 0 === i.error;
                    else if (!n) return !1;
                    if (n) {
                        var o;
                        if ((t.length > 0 && (o = t[0]), o instanceof Error)) throw o;
                        var s = new Error("Unhandled error." + (o ? " (" + o.message + ")" : ""));
                        throw ((s.context = o), s);
                    }
                    var c = i[e];
                    if (void 0 === c) return !1;
                    if ("function" == typeof c) a(c, this, t);
                    else {
                        var u = c.length,
                            l = m(c, u);
                        for (r = 0; r < u; ++r) a(l[r], this, t);
                    }
                    return !0;
                }),
                (s.prototype.addListener = function (e, t) {
                    return d(this, e, t, !1);
                }),
                (s.prototype.on = s.prototype.addListener),
                (s.prototype.prependListener = function (e, t) {
                    return d(this, e, t, !0);
                }),
                (s.prototype.once = function (e, t) {
                    return u(t), this.on(e, p(this, e, t)), this;
                }),
                (s.prototype.prependOnceListener = function (e, t) {
                    return u(t), this.prependListener(e, p(this, e, t)), this;
                }),
                (s.prototype.removeListener = function (e, t) {
                    var r, n, i, a, o;
                    if ((u(t), void 0 === (n = this._events))) return this;
                    if (void 0 === (r = n[e])) return this;
                    if (r === t || r.listener === t) 0 == --this._eventsCount ? (this._events = Object.create(null)) : (delete n[e], n.removeListener && this.emit("removeListener", e, r.listener || t));
                    else if ("function" != typeof r) {
                        for (i = -1, a = r.length - 1; a >= 0; a--)
                            if (r[a] === t || r[a].listener === t) {
                                (o = r[a].listener), (i = a);
                                break;
                            }
                        if (i < 0) return this;
                        0 === i
                            ? r.shift()
                            : (function (e, t) {
                                for (; t + 1 < e.length; t++) e[t] = e[t + 1];
                                e.pop();
                            })(r, i),
                            1 === r.length && (n[e] = r[0]),
                            void 0 !== n.removeListener && this.emit("removeListener", e, o || t);
                    }
                    return this;
                }),
                (s.prototype.off = s.prototype.removeListener),
                (s.prototype.removeAllListeners = function (e) {
                    var t, r, n;
                    if (void 0 === (r = this._events)) return this;
                    if (void 0 === r.removeListener)
                        return 0 === arguments.length ? ((this._events = Object.create(null)), (this._eventsCount = 0)) : void 0 !== r[e] && (0 == --this._eventsCount ? (this._events = Object.create(null)) : delete r[e]), this;
                    if (0 === arguments.length) {
                        var i,
                            a = Object.keys(r);
                        for (n = 0; n < a.length; ++n) "removeListener" !== (i = a[n]) && this.removeAllListeners(i);
                        return this.removeAllListeners("removeListener"), (this._events = Object.create(null)), (this._eventsCount = 0), this;
                    }
                    if ("function" == typeof (t = r[e])) this.removeListener(e, t);
                    else if (void 0 !== t) for (n = t.length - 1; n >= 0; n--) this.removeListener(e, t[n]);
                    return this;
                }),
                (s.prototype.listeners = function (e) {
                    return h(this, e, !0);
                }),
                (s.prototype.rawListeners = function (e) {
                    return h(this, e, !1);
                }),
                (s.listenerCount = function (e, t) {
                    return "function" == typeof e.listenerCount ? e.listenerCount(t) : v.call(e, t);
                }),
                (s.prototype.listenerCount = v),
                (s.prototype.eventNames = function () {
                    return this._eventsCount > 0 ? n(this._events) : [];
                });
        },
        function (e, t, r) {
            var n = r(94),
                i = r(97);
            e.exports = function (e, t) {
                var r = i(e, t);
                return n(r) ? r : void 0;
            };
        },
        function (e, t) {
            e.exports = function (e, t, r) {
                return t in e ? Object.defineProperty(e, t, { value: r, enumerable: !0, configurable: !0, writable: !0 }) : (e[t] = r), e;
            };
        },
        function (e, t, r) {
            var n = r(20),
                i = r(72),
                a = r(73),
                o = n ? n.toStringTag : void 0;
            e.exports = function (e) {
                return null == e ? (void 0 === e ? "[object Undefined]" : "[object Null]") : o && o in Object(e) ? i(e) : a(e);
            };
        },
        function (e, t) {
            e.exports = function (e) {
                return null != e && "object" == typeof e;
            };
        },
        function (e, t) {
            function r(t) {
                return (
                    "function" == typeof Symbol && "symbol" == typeof Symbol.iterator
                        ? (e.exports = r = function (e) {
                            return typeof e;
                        })
                        : (e.exports = r = function (e) {
                            return e && "function" == typeof Symbol && e.constructor === Symbol && e !== Symbol.prototype ? "symbol" : typeof e;
                        }),
                    r(t)
                );
            }
            e.exports = r;
        },
        function (e, t) {
            e.exports = function (e) {
                if (void 0 === e) throw new ReferenceError("this hasn't been initialised - super() hasn't been called");
                return e;
            };
        },
        function (e, t, r) {
            var n = r(37),
                i = r(65),
                a = r(48),
                o = r(6);
            e.exports = function (e, t) {
                return (o(e) ? n : i)(e, a(t, 3));
            };
        },
        function (e, t, r) {
            var n = r(145),
                i = r(6);
            e.exports = function (e, t, r, a) {
                return null == e ? [] : (i(t) || (t = null == t ? [] : [t]), i((r = a ? void 0 : r)) || (r = null == r ? [] : [r]), n(e, t, r));
            };
        },
        function (e, t, r) {
            var n = r(7).Symbol;
            e.exports = n;
        },
        function (e, t, r) {
            var n = r(84),
                i = r(85),
                a = r(86),
                o = r(87),
                s = r(88);
            function c(e) {
                var t = -1,
                    r = null == e ? 0 : e.length;
                for (this.clear(); ++t < r;) {
                    var n = e[t];
                    this.set(n[0], n[1]);
                }
            }
            (c.prototype.clear = n), (c.prototype.delete = i), (c.prototype.get = a), (c.prototype.has = o), (c.prototype.set = s), (e.exports = c);
        },
        function (e, t, r) {
            var n = r(50);
            e.exports = function (e, t) {
                for (var r = e.length; r--;) if (n(e[r][0], t)) return r;
                return -1;
            };
        },
        function (e, t, r) {
            var n = r(12)(Object, "create");
            e.exports = n;
        },
        function (e, t, r) {
            var n = r(106);
            e.exports = function (e, t) {
                var r = e.__data__;
                return n(t) ? r["string" == typeof t ? "string" : "hash"] : r.map;
            };
        },
        function (e, t, r) {
            var n = r(14),
                i = r(15);
            e.exports = function (e) {
                return "symbol" == typeof e || (i(e) && "[object Symbol]" == n(e));
            };
        },
        function (e, t, r) {
            var n = r(25);
            e.exports = function (e) {
                if ("string" == typeof e || n(e)) return e;
                var t = e + "";
                return "0" == t && 1 / e == -1 / 0 ? "-0" : t;
            };
        },
        function (e, t, r) {
            "use strict";
            r.d(t, "a", function () {
                return c;
            });
            var n = r(3),
                i = r.n(n),
                a = r(4),
                o = r.n(a),
                s = r(5),
                c = (function () {
                    function e() {
                        i()(this, e);
                    }
                    return (
                        o()(e, [
                            {
                                key: "addListenerForMessagesFromCallMachine",
                                value: function (e, t, r) {
                                    Object(s.b)();
                                },
                            },
                            {
                                key: "addListenerForMessagesFromDailyJs",
                                value: function (e, t, r) {
                                    Object(s.b)();
                                },
                            },
                            {
                                key: "sendMessageToCallMachine",
                                value: function (e, t, r, n) {
                                    Object(s.b)();
                                },
                            },
                            {
                                key: "sendMessageToDailyJs",
                                value: function (e, t, r) {
                                    Object(s.b)();
                                },
                            },
                            {
                                key: "removeListener",
                                value: function (e) {
                                    Object(s.b)();
                                },
                            },
                        ]),
                        e
                    );
                })();
        },
        function (e, t) {
            function r(t, n) {
                return (
                    (e.exports = r =
                        Object.setPrototypeOf ||
                        function (e, t) {
                            return (e.__proto__ = t), e;
                        }),
                    r(t, n)
                );
            }
            e.exports = r;
        },
        function (e, t, r) {
            var n = r(69),
                i = r(77),
                a = r(31);
            e.exports = function (e) {
                return a(e) ? n(e) : i(e);
            };
        },
        function (e, t) {
            e.exports = function (e) {
                return "number" == typeof e && e > -1 && e % 1 == 0 && e <= 9007199254740991;
            };
        },
        function (e, t, r) {
            var n = r(47),
                i = r(30);
            e.exports = function (e) {
                return null != e && i(e.length) && !n(e);
            };
        },
        function (e, t) {
            e.exports = function (e) {
                var t = typeof e;
                return null != e && ("object" == t || "function" == t);
            };
        },
        function (e, t, r) {
            var n = r(12)(r(7), "Map");
            e.exports = n;
        },
        function (e, t, r) {
            var n = r(98),
                i = r(105),
                a = r(107),
                o = r(108),
                s = r(109);
            function c(e) {
                var t = -1,
                    r = null == e ? 0 : e.length;
                for (this.clear(); ++t < r;) {
                    var n = e[t];
                    this.set(n[0], n[1]);
                }
            }
            (c.prototype.clear = n), (c.prototype.delete = i), (c.prototype.get = a), (c.prototype.has = o), (c.prototype.set = s), (e.exports = c);
        },
        function (e, t, r) {
            var n = r(6),
                i = r(25),
                a = /\.|\[(?:[^[\]]*|(["'])(?:(?!\1)[^\\]|\\.)*?\1)\]/,
                o = /^\w*$/;
            e.exports = function (e, t) {
                if (n(e)) return !1;
                var r = typeof e;
                return !("number" != r && "symbol" != r && "boolean" != r && null != e && !i(e)) || o.test(e) || !a.test(e) || (null != t && e in Object(t));
            };
        },
        function (e, t, r) {
            e.exports = (function (e) {
                var t = {};
                function r(n) {
                    if (t[n]) return t[n].exports;
                    var i = (t[n] = { i: n, l: !1, exports: {} });
                    return e[n].call(i.exports, i, i.exports, r), (i.l = !0), i.exports;
                }
                return (
                    (r.m = e),
                    (r.c = t),
                    (r.d = function (e, t, n) {
                        r.o(e, t) || Object.defineProperty(e, t, { enumerable: !0, get: n });
                    }),
                    (r.r = function (e) {
                        "undefined" != typeof Symbol && Symbol.toStringTag && Object.defineProperty(e, Symbol.toStringTag, { value: "Module" }), Object.defineProperty(e, "__esModule", { value: !0 });
                    }),
                    (r.t = function (e, t) {
                        if ((1 & t && (e = r(e)), 8 & t)) return e;
                        if (4 & t && "object" == typeof e && e && e.__esModule) return e;
                        var n = Object.create(null);
                        if ((r.r(n), Object.defineProperty(n, "default", { enumerable: !0, value: e }), 2 & t && "string" != typeof e))
                            for (var i in e)
                                r.d(
                                    n,
                                    i,
                                    function (t) {
                                        return e[t];
                                    }.bind(null, i)
                                );
                        return n;
                    }),
                    (r.n = function (e) {
                        var t =
                            e && e.__esModule
                                ? function () {
                                    return e.default;
                                }
                                : function () {
                                    return e;
                                };
                        return r.d(t, "a", t), t;
                    }),
                    (r.o = function (e, t) {
                        return Object.prototype.hasOwnProperty.call(e, t);
                    }),
                    (r.p = ""),
                    r((r.s = 90))
                );
            })({
                17: function (e, t, r) {
                    "use strict";
                    (t.__esModule = !0), (t.default = void 0);
                    var n = r(18),
                        i = (function () {
                            function e() { }
                            return (
                                (e.getFirstMatch = function (e, t) {
                                    var r = t.match(e);
                                    return (r && r.length > 0 && r[1]) || "";
                                }),
                                (e.getSecondMatch = function (e, t) {
                                    var r = t.match(e);
                                    return (r && r.length > 1 && r[2]) || "";
                                }),
                                (e.matchAndReturnConst = function (e, t, r) {
                                    if (e.test(t)) return r;
                                }),
                                (e.getWindowsVersionName = function (e) {
                                    switch (e) {
                                        case "NT":
                                            return "NT";
                                        case "XP":
                                            return "XP";
                                        case "NT 5.0":
                                            return "2000";
                                        case "NT 5.1":
                                            return "XP";
                                        case "NT 5.2":
                                            return "2003";
                                        case "NT 6.0":
                                            return "Vista";
                                        case "NT 6.1":
                                            return "7";
                                        case "NT 6.2":
                                            return "8";
                                        case "NT 6.3":
                                            return "8.1";
                                        case "NT 10.0":
                                            return "10";
                                        default:
                                            return;
                                    }
                                }),
                                (e.getMacOSVersionName = function (e) {
                                    var t = e
                                        .split(".")
                                        .splice(0, 2)
                                        .map(function (e) {
                                            return parseInt(e, 10) || 0;
                                        });
                                    if ((t.push(0), 10 === t[0]))
                                        switch (t[1]) {
                                            case 5:
                                                return "Leopard";
                                            case 6:
                                                return "Snow Leopard";
                                            case 7:
                                                return "Lion";
                                            case 8:
                                                return "Mountain Lion";
                                            case 9:
                                                return "Mavericks";
                                            case 10:
                                                return "Yosemite";
                                            case 11:
                                                return "El Capitan";
                                            case 12:
                                                return "Sierra";
                                            case 13:
                                                return "High Sierra";
                                            case 14:
                                                return "Mojave";
                                            case 15:
                                                return "Catalina";
                                            default:
                                                return;
                                        }
                                }),
                                (e.getAndroidVersionName = function (e) {
                                    var t = e
                                        .split(".")
                                        .splice(0, 2)
                                        .map(function (e) {
                                            return parseInt(e, 10) || 0;
                                        });
                                    if ((t.push(0), !(1 === t[0] && t[1] < 5)))
                                        return 1 === t[0] && t[1] < 6
                                            ? "Cupcake"
                                            : 1 === t[0] && t[1] >= 6
                                                ? "Donut"
                                                : 2 === t[0] && t[1] < 2
                                                    ? "Eclair"
                                                    : 2 === t[0] && 2 === t[1]
                                                        ? "Froyo"
                                                        : 2 === t[0] && t[1] > 2
                                                            ? "Gingerbread"
                                                            : 3 === t[0]
                                                                ? "Honeycomb"
                                                                : 4 === t[0] && t[1] < 1
                                                                    ? "Ice Cream Sandwich"
                                                                    : 4 === t[0] && t[1] < 4
                                                                        ? "Jelly Bean"
                                                                        : 4 === t[0] && t[1] >= 4
                                                                            ? "KitKat"
                                                                            : 5 === t[0]
                                                                                ? "Lollipop"
                                                                                : 6 === t[0]
                                                                                    ? "Marshmallow"
                                                                                    : 7 === t[0]
                                                                                        ? "Nougat"
                                                                                        : 8 === t[0]
                                                                                            ? "Oreo"
                                                                                            : 9 === t[0]
                                                                                                ? "Pie"
                                                                                                : void 0;
                                }),
                                (e.getVersionPrecision = function (e) {
                                    return e.split(".").length;
                                }),
                                (e.compareVersions = function (t, r, n) {
                                    void 0 === n && (n = !1);
                                    var i = e.getVersionPrecision(t),
                                        a = e.getVersionPrecision(r),
                                        o = Math.max(i, a),
                                        s = 0,
                                        c = e.map([t, r], function (t) {
                                            var r = o - e.getVersionPrecision(t),
                                                n = t + new Array(r + 1).join(".0");
                                            return e
                                                .map(n.split("."), function (e) {
                                                    return new Array(20 - e.length).join("0") + e;
                                                })
                                                .reverse();
                                        });
                                    for (n && (s = o - Math.min(i, a)), o -= 1; o >= s;) {
                                        if (c[0][o] > c[1][o]) return 1;
                                        if (c[0][o] === c[1][o]) {
                                            if (o === s) return 0;
                                            o -= 1;
                                        } else if (c[0][o] < c[1][o]) return -1;
                                    }
                                }),
                                (e.map = function (e, t) {
                                    var r,
                                        n = [];
                                    if (Array.prototype.map) return Array.prototype.map.call(e, t);
                                    for (r = 0; r < e.length; r += 1) n.push(t(e[r]));
                                    return n;
                                }),
                                (e.find = function (e, t) {
                                    var r, n;
                                    if (Array.prototype.find) return Array.prototype.find.call(e, t);
                                    for (r = 0, n = e.length; r < n; r += 1) {
                                        var i = e[r];
                                        if (t(i, r)) return i;
                                    }
                                }),
                                (e.assign = function (e) {
                                    for (var t, r, n = e, i = arguments.length, a = new Array(i > 1 ? i - 1 : 0), o = 1; o < i; o++) a[o - 1] = arguments[o];
                                    if (Object.assign) return Object.assign.apply(Object, [e].concat(a));
                                    var s = function () {
                                        var e = a[t];
                                        "object" == typeof e &&
                                            null !== e &&
                                            Object.keys(e).forEach(function (t) {
                                                n[t] = e[t];
                                            });
                                    };
                                    for (t = 0, r = a.length; t < r; t += 1) s();
                                    return e;
                                }),
                                (e.getBrowserAlias = function (e) {
                                    return n.BROWSER_ALIASES_MAP[e];
                                }),
                                (e.getBrowserTypeByAlias = function (e) {
                                    return n.BROWSER_MAP[e] || "";
                                }),
                                e
                            );
                        })();
                    (t.default = i), (e.exports = t.default);
                },
                18: function (e, t, r) {
                    "use strict";
                    (t.__esModule = !0),
                        (t.ENGINE_MAP = t.OS_MAP = t.PLATFORMS_MAP = t.BROWSER_MAP = t.BROWSER_ALIASES_MAP = void 0),
                        (t.BROWSER_ALIASES_MAP = {
                            "Amazon Silk": "amazon_silk",
                            "Android Browser": "android",
                            Bada: "bada",
                            BlackBerry: "blackberry",
                            Chrome: "chrome",
                            Chromium: "chromium",
                            Electron: "electron",
                            Epiphany: "epiphany",
                            Firefox: "firefox",
                            Focus: "focus",
                            Generic: "generic",
                            "Google Search": "google_search",
                            Googlebot: "googlebot",
                            "Internet Explorer": "ie",
                            "K-Meleon": "k_meleon",
                            Maxthon: "maxthon",
                            "Microsoft Edge": "edge",
                            "MZ Browser": "mz",
                            "NAVER Whale Browser": "naver",
                            Opera: "opera",
                            "Opera Coast": "opera_coast",
                            PhantomJS: "phantomjs",
                            Puffin: "puffin",
                            QupZilla: "qupzilla",
                            QQ: "qq",
                            QQLite: "qqlite",
                            Safari: "safari",
                            Sailfish: "sailfish",
                            "Samsung Internet for Android": "samsung_internet",
                            SeaMonkey: "seamonkey",
                            Sleipnir: "sleipnir",
                            Swing: "swing",
                            Tizen: "tizen",
                            "UC Browser": "uc",
                            Vivaldi: "vivaldi",
                            "WebOS Browser": "webos",
                            WeChat: "wechat",
                            "Yandex Browser": "yandex",
                            Roku: "roku",
                        }),
                        (t.BROWSER_MAP = {
                            amazon_silk: "Amazon Silk",
                            android: "Android Browser",
                            bada: "Bada",
                            blackberry: "BlackBerry",
                            chrome: "Chrome",
                            chromium: "Chromium",
                            electron: "Electron",
                            epiphany: "Epiphany",
                            firefox: "Firefox",
                            focus: "Focus",
                            generic: "Generic",
                            googlebot: "Googlebot",
                            google_search: "Google Search",
                            ie: "Internet Explorer",
                            k_meleon: "K-Meleon",
                            maxthon: "Maxthon",
                            edge: "Microsoft Edge",
                            mz: "MZ Browser",
                            naver: "NAVER Whale Browser",
                            opera: "Opera",
                            opera_coast: "Opera Coast",
                            phantomjs: "PhantomJS",
                            puffin: "Puffin",
                            qupzilla: "QupZilla",
                            qq: "QQ Browser",
                            qqlite: "QQ Browser Lite",
                            safari: "Safari",
                            sailfish: "Sailfish",
                            samsung_internet: "Samsung Internet for Android",
                            seamonkey: "SeaMonkey",
                            sleipnir: "Sleipnir",
                            swing: "Swing",
                            tizen: "Tizen",
                            uc: "UC Browser",
                            vivaldi: "Vivaldi",
                            webos: "WebOS Browser",
                            wechat: "WeChat",
                            yandex: "Yandex Browser",
                        }),
                        (t.PLATFORMS_MAP = { tablet: "tablet", mobile: "mobile", desktop: "desktop", tv: "tv" }),
                        (t.OS_MAP = {
                            WindowsPhone: "Windows Phone",
                            Windows: "Windows",
                            MacOS: "macOS",
                            iOS: "iOS",
                            Android: "Android",
                            WebOS: "WebOS",
                            BlackBerry: "BlackBerry",
                            Bada: "Bada",
                            Tizen: "Tizen",
                            Linux: "Linux",
                            ChromeOS: "Chrome OS",
                            PlayStation4: "PlayStation 4",
                            Roku: "Roku",
                        }),
                        (t.ENGINE_MAP = { EdgeHTML: "EdgeHTML", Blink: "Blink", Trident: "Trident", Presto: "Presto", Gecko: "Gecko", WebKit: "WebKit" });
                },
                90: function (e, t, r) {
                    "use strict";
                    (t.__esModule = !0), (t.default = void 0);
                    var n,
                        i = (n = r(91)) && n.__esModule ? n : { default: n },
                        a = r(18);
                    function o(e, t) {
                        for (var r = 0; r < t.length; r++) {
                            var n = t[r];
                            (n.enumerable = n.enumerable || !1), (n.configurable = !0), "value" in n && (n.writable = !0), Object.defineProperty(e, n.key, n);
                        }
                    }
                    var s = (function () {
                        function e() { }
                        var t, r;
                        return (
                            (e.getParser = function (e, t) {
                                if ((void 0 === t && (t = !1), "string" != typeof e)) throw new Error("UserAgent should be a string");
                                return new i.default(e, t);
                            }),
                            (e.parse = function (e) {
                                return new i.default(e).getResult();
                            }),
                            (t = e),
                            (r = [
                                {
                                    key: "BROWSER_MAP",
                                    get: function () {
                                        return a.BROWSER_MAP;
                                    },
                                },
                                {
                                    key: "ENGINE_MAP",
                                    get: function () {
                                        return a.ENGINE_MAP;
                                    },
                                },
                                {
                                    key: "OS_MAP",
                                    get: function () {
                                        return a.OS_MAP;
                                    },
                                },
                                {
                                    key: "PLATFORMS_MAP",
                                    get: function () {
                                        return a.PLATFORMS_MAP;
                                    },
                                },
                            ]) && o(t, r),
                            e
                        );
                    })();
                    (t.default = s), (e.exports = t.default);
                },
                91: function (e, t, r) {
                    "use strict";
                    (t.__esModule = !0), (t.default = void 0);
                    var n = c(r(92)),
                        i = c(r(93)),
                        a = c(r(94)),
                        o = c(r(95)),
                        s = c(r(17));
                    function c(e) {
                        return e && e.__esModule ? e : { default: e };
                    }
                    var u = (function () {
                        function e(e, t) {
                            if ((void 0 === t && (t = !1), null == e || "" === e)) throw new Error("UserAgent parameter can't be empty");
                            (this._ua = e), (this.parsedResult = {}), !0 !== t && this.parse();
                        }
                        var t = e.prototype;
                        return (
                            (t.getUA = function () {
                                return this._ua;
                            }),
                            (t.test = function (e) {
                                return e.test(this._ua);
                            }),
                            (t.parseBrowser = function () {
                                var e = this;
                                this.parsedResult.browser = {};
                                var t = s.default.find(n.default, function (t) {
                                    if ("function" == typeof t.test) return t.test(e);
                                    if (t.test instanceof Array)
                                        return t.test.some(function (t) {
                                            return e.test(t);
                                        });
                                    throw new Error("Browser's test function is not valid");
                                });
                                return t && (this.parsedResult.browser = t.describe(this.getUA())), this.parsedResult.browser;
                            }),
                            (t.getBrowser = function () {
                                return this.parsedResult.browser ? this.parsedResult.browser : this.parseBrowser();
                            }),
                            (t.getBrowserName = function (e) {
                                return e ? String(this.getBrowser().name).toLowerCase() || "" : this.getBrowser().name || "";
                            }),
                            (t.getBrowserVersion = function () {
                                return this.getBrowser().version;
                            }),
                            (t.getOS = function () {
                                return this.parsedResult.os ? this.parsedResult.os : this.parseOS();
                            }),
                            (t.parseOS = function () {
                                var e = this;
                                this.parsedResult.os = {};
                                var t = s.default.find(i.default, function (t) {
                                    if ("function" == typeof t.test) return t.test(e);
                                    if (t.test instanceof Array)
                                        return t.test.some(function (t) {
                                            return e.test(t);
                                        });
                                    throw new Error("Browser's test function is not valid");
                                });
                                return t && (this.parsedResult.os = t.describe(this.getUA())), this.parsedResult.os;
                            }),
                            (t.getOSName = function (e) {
                                var t = this.getOS().name;
                                return e ? String(t).toLowerCase() || "" : t || "";
                            }),
                            (t.getOSVersion = function () {
                                return this.getOS().version;
                            }),
                            (t.getPlatform = function () {
                                return this.parsedResult.platform ? this.parsedResult.platform : this.parsePlatform();
                            }),
                            (t.getPlatformType = function (e) {
                                void 0 === e && (e = !1);
                                var t = this.getPlatform().type;
                                return e ? String(t).toLowerCase() || "" : t || "";
                            }),
                            (t.parsePlatform = function () {
                                var e = this;
                                this.parsedResult.platform = {};
                                var t = s.default.find(a.default, function (t) {
                                    if ("function" == typeof t.test) return t.test(e);
                                    if (t.test instanceof Array)
                                        return t.test.some(function (t) {
                                            return e.test(t);
                                        });
                                    throw new Error("Browser's test function is not valid");
                                });
                                return t && (this.parsedResult.platform = t.describe(this.getUA())), this.parsedResult.platform;
                            }),
                            (t.getEngine = function () {
                                return this.parsedResult.engine ? this.parsedResult.engine : this.parseEngine();
                            }),
                            (t.getEngineName = function (e) {
                                return e ? String(this.getEngine().name).toLowerCase() || "" : this.getEngine().name || "";
                            }),
                            (t.parseEngine = function () {
                                var e = this;
                                this.parsedResult.engine = {};
                                var t = s.default.find(o.default, function (t) {
                                    if ("function" == typeof t.test) return t.test(e);
                                    if (t.test instanceof Array)
                                        return t.test.some(function (t) {
                                            return e.test(t);
                                        });
                                    throw new Error("Browser's test function is not valid");
                                });
                                return t && (this.parsedResult.engine = t.describe(this.getUA())), this.parsedResult.engine;
                            }),
                            (t.parse = function () {
                                return this.parseBrowser(), this.parseOS(), this.parsePlatform(), this.parseEngine(), this;
                            }),
                            (t.getResult = function () {
                                return s.default.assign({}, this.parsedResult);
                            }),
                            (t.satisfies = function (e) {
                                var t = this,
                                    r = {},
                                    n = 0,
                                    i = {},
                                    a = 0;
                                if (
                                    (Object.keys(e).forEach(function (t) {
                                        var o = e[t];
                                        "string" == typeof o ? ((i[t] = o), (a += 1)) : "object" == typeof o && ((r[t] = o), (n += 1));
                                    }),
                                        n > 0)
                                ) {
                                    var o = Object.keys(r),
                                        c = s.default.find(o, function (e) {
                                            return t.isOS(e);
                                        });
                                    if (c) {
                                        var u = this.satisfies(r[c]);
                                        if (void 0 !== u) return u;
                                    }
                                    var l = s.default.find(o, function (e) {
                                        return t.isPlatform(e);
                                    });
                                    if (l) {
                                        var d = this.satisfies(r[l]);
                                        if (void 0 !== d) return d;
                                    }
                                }
                                if (a > 0) {
                                    var f = Object.keys(i),
                                        p = s.default.find(f, function (e) {
                                            return t.isBrowser(e, !0);
                                        });
                                    if (void 0 !== p) return this.compareVersion(i[p]);
                                }
                            }),
                            (t.isBrowser = function (e, t) {
                                void 0 === t && (t = !1);
                                var r = this.getBrowserName().toLowerCase(),
                                    n = e.toLowerCase(),
                                    i = s.default.getBrowserTypeByAlias(n);
                                return t && i && (n = i.toLowerCase()), n === r;
                            }),
                            (t.compareVersion = function (e) {
                                var t = [0],
                                    r = e,
                                    n = !1,
                                    i = this.getBrowserVersion();
                                if ("string" == typeof i)
                                    return (
                                        ">" === e[0] || "<" === e[0]
                                            ? ((r = e.substr(1)), "=" === e[1] ? ((n = !0), (r = e.substr(2))) : (t = []), ">" === e[0] ? t.push(1) : t.push(-1))
                                            : "=" === e[0]
                                                ? (r = e.substr(1))
                                                : "~" === e[0] && ((n = !0), (r = e.substr(1))),
                                        t.indexOf(s.default.compareVersions(i, r, n)) > -1
                                    );
                            }),
                            (t.isOS = function (e) {
                                return this.getOSName(!0) === String(e).toLowerCase();
                            }),
                            (t.isPlatform = function (e) {
                                return this.getPlatformType(!0) === String(e).toLowerCase();
                            }),
                            (t.isEngine = function (e) {
                                return this.getEngineName(!0) === String(e).toLowerCase();
                            }),
                            (t.is = function (e) {
                                return this.isBrowser(e) || this.isOS(e) || this.isPlatform(e);
                            }),
                            (t.some = function (e) {
                                var t = this;
                                return (
                                    void 0 === e && (e = []),
                                    e.some(function (e) {
                                        return t.is(e);
                                    })
                                );
                            }),
                            e
                        );
                    })();
                    (t.default = u), (e.exports = t.default);
                },
                92: function (e, t, r) {
                    "use strict";
                    (t.__esModule = !0), (t.default = void 0);
                    var n,
                        i = (n = r(17)) && n.__esModule ? n : { default: n },
                        a = /version\/(\d+(\.?_?\d+)+)/i,
                        o = [
                            {
                                test: [/googlebot/i],
                                describe: function (e) {
                                    var t = { name: "Googlebot" },
                                        r = i.default.getFirstMatch(/googlebot\/(\d+(\.\d+))/i, e) || i.default.getFirstMatch(a, e);
                                    return r && (t.version = r), t;
                                },
                            },
                            {
                                test: [/opera/i],
                                describe: function (e) {
                                    var t = { name: "Opera" },
                                        r = i.default.getFirstMatch(a, e) || i.default.getFirstMatch(/(?:opera)[\s/](\d+(\.?_?\d+)+)/i, e);
                                    return r && (t.version = r), t;
                                },
                            },
                            {
                                test: [/opr\/|opios/i],
                                describe: function (e) {
                                    var t = { name: "Opera" },
                                        r = i.default.getFirstMatch(/(?:opr|opios)[\s/](\S+)/i, e) || i.default.getFirstMatch(a, e);
                                    return r && (t.version = r), t;
                                },
                            },
                            {
                                test: [/SamsungBrowser/i],
                                describe: function (e) {
                                    var t = { name: "Samsung Internet for Android" },
                                        r = i.default.getFirstMatch(a, e) || i.default.getFirstMatch(/(?:SamsungBrowser)[\s/](\d+(\.?_?\d+)+)/i, e);
                                    return r && (t.version = r), t;
                                },
                            },
                            {
                                test: [/Whale/i],
                                describe: function (e) {
                                    var t = { name: "NAVER Whale Browser" },
                                        r = i.default.getFirstMatch(a, e) || i.default.getFirstMatch(/(?:whale)[\s/](\d+(?:\.\d+)+)/i, e);
                                    return r && (t.version = r), t;
                                },
                            },
                            {
                                test: [/MZBrowser/i],
                                describe: function (e) {
                                    var t = { name: "MZ Browser" },
                                        r = i.default.getFirstMatch(/(?:MZBrowser)[\s/](\d+(?:\.\d+)+)/i, e) || i.default.getFirstMatch(a, e);
                                    return r && (t.version = r), t;
                                },
                            },
                            {
                                test: [/focus/i],
                                describe: function (e) {
                                    var t = { name: "Focus" },
                                        r = i.default.getFirstMatch(/(?:focus)[\s/](\d+(?:\.\d+)+)/i, e) || i.default.getFirstMatch(a, e);
                                    return r && (t.version = r), t;
                                },
                            },
                            {
                                test: [/swing/i],
                                describe: function (e) {
                                    var t = { name: "Swing" },
                                        r = i.default.getFirstMatch(/(?:swing)[\s/](\d+(?:\.\d+)+)/i, e) || i.default.getFirstMatch(a, e);
                                    return r && (t.version = r), t;
                                },
                            },
                            {
                                test: [/coast/i],
                                describe: function (e) {
                                    var t = { name: "Opera Coast" },
                                        r = i.default.getFirstMatch(a, e) || i.default.getFirstMatch(/(?:coast)[\s/](\d+(\.?_?\d+)+)/i, e);
                                    return r && (t.version = r), t;
                                },
                            },
                            {
                                test: [/yabrowser/i],
                                describe: function (e) {
                                    var t = { name: "Yandex Browser" },
                                        r = i.default.getFirstMatch(/(?:yabrowser)[\s/](\d+(\.?_?\d+)+)/i, e) || i.default.getFirstMatch(a, e);
                                    return r && (t.version = r), t;
                                },
                            },
                            {
                                test: [/ucbrowser/i],
                                describe: function (e) {
                                    var t = { name: "UC Browser" },
                                        r = i.default.getFirstMatch(a, e) || i.default.getFirstMatch(/(?:ucbrowser)[\s/](\d+(\.?_?\d+)+)/i, e);
                                    return r && (t.version = r), t;
                                },
                            },
                            {
                                test: [/Maxthon|mxios/i],
                                describe: function (e) {
                                    var t = { name: "Maxthon" },
                                        r = i.default.getFirstMatch(a, e) || i.default.getFirstMatch(/(?:Maxthon|mxios)[\s/](\d+(\.?_?\d+)+)/i, e);
                                    return r && (t.version = r), t;
                                },
                            },
                            {
                                test: [/epiphany/i],
                                describe: function (e) {
                                    var t = { name: "Epiphany" },
                                        r = i.default.getFirstMatch(a, e) || i.default.getFirstMatch(/(?:epiphany)[\s/](\d+(\.?_?\d+)+)/i, e);
                                    return r && (t.version = r), t;
                                },
                            },
                            {
                                test: [/puffin/i],
                                describe: function (e) {
                                    var t = { name: "Puffin" },
                                        r = i.default.getFirstMatch(a, e) || i.default.getFirstMatch(/(?:puffin)[\s/](\d+(\.?_?\d+)+)/i, e);
                                    return r && (t.version = r), t;
                                },
                            },
                            {
                                test: [/sleipnir/i],
                                describe: function (e) {
                                    var t = { name: "Sleipnir" },
                                        r = i.default.getFirstMatch(a, e) || i.default.getFirstMatch(/(?:sleipnir)[\s/](\d+(\.?_?\d+)+)/i, e);
                                    return r && (t.version = r), t;
                                },
                            },
                            {
                                test: [/k-meleon/i],
                                describe: function (e) {
                                    var t = { name: "K-Meleon" },
                                        r = i.default.getFirstMatch(a, e) || i.default.getFirstMatch(/(?:k-meleon)[\s/](\d+(\.?_?\d+)+)/i, e);
                                    return r && (t.version = r), t;
                                },
                            },
                            {
                                test: [/micromessenger/i],
                                describe: function (e) {
                                    var t = { name: "WeChat" },
                                        r = i.default.getFirstMatch(/(?:micromessenger)[\s/](\d+(\.?_?\d+)+)/i, e) || i.default.getFirstMatch(a, e);
                                    return r && (t.version = r), t;
                                },
                            },
                            {
                                test: [/qqbrowser/i],
                                describe: function (e) {
                                    var t = { name: /qqbrowserlite/i.test(e) ? "QQ Browser Lite" : "QQ Browser" },
                                        r = i.default.getFirstMatch(/(?:qqbrowserlite|qqbrowser)[/](\d+(\.?_?\d+)+)/i, e) || i.default.getFirstMatch(a, e);
                                    return r && (t.version = r), t;
                                },
                            },
                            {
                                test: [/msie|trident/i],
                                describe: function (e) {
                                    var t = { name: "Internet Explorer" },
                                        r = i.default.getFirstMatch(/(?:msie |rv:)(\d+(\.?_?\d+)+)/i, e);
                                    return r && (t.version = r), t;
                                },
                            },
                            {
                                test: [/\sedg\//i],
                                describe: function (e) {
                                    var t = { name: "Microsoft Edge" },
                                        r = i.default.getFirstMatch(/\sedg\/(\d+(\.?_?\d+)+)/i, e);
                                    return r && (t.version = r), t;
                                },
                            },
                            {
                                test: [/edg([ea]|ios)/i],
                                describe: function (e) {
                                    var t = { name: "Microsoft Edge" },
                                        r = i.default.getSecondMatch(/edg([ea]|ios)\/(\d+(\.?_?\d+)+)/i, e);
                                    return r && (t.version = r), t;
                                },
                            },
                            {
                                test: [/vivaldi/i],
                                describe: function (e) {
                                    var t = { name: "Vivaldi" },
                                        r = i.default.getFirstMatch(/vivaldi\/(\d+(\.?_?\d+)+)/i, e);
                                    return r && (t.version = r), t;
                                },
                            },
                            {
                                test: [/seamonkey/i],
                                describe: function (e) {
                                    var t = { name: "SeaMonkey" },
                                        r = i.default.getFirstMatch(/seamonkey\/(\d+(\.?_?\d+)+)/i, e);
                                    return r && (t.version = r), t;
                                },
                            },
                            {
                                test: [/sailfish/i],
                                describe: function (e) {
                                    var t = { name: "Sailfish" },
                                        r = i.default.getFirstMatch(/sailfish\s?browser\/(\d+(\.\d+)?)/i, e);
                                    return r && (t.version = r), t;
                                },
                            },
                            {
                                test: [/silk/i],
                                describe: function (e) {
                                    var t = { name: "Amazon Silk" },
                                        r = i.default.getFirstMatch(/silk\/(\d+(\.?_?\d+)+)/i, e);
                                    return r && (t.version = r), t;
                                },
                            },
                            {
                                test: [/phantom/i],
                                describe: function (e) {
                                    var t = { name: "PhantomJS" },
                                        r = i.default.getFirstMatch(/phantomjs\/(\d+(\.?_?\d+)+)/i, e);
                                    return r && (t.version = r), t;
                                },
                            },
                            {
                                test: [/slimerjs/i],
                                describe: function (e) {
                                    var t = { name: "SlimerJS" },
                                        r = i.default.getFirstMatch(/slimerjs\/(\d+(\.?_?\d+)+)/i, e);
                                    return r && (t.version = r), t;
                                },
                            },
                            {
                                test: [/blackberry|\bbb\d+/i, /rim\stablet/i],
                                describe: function (e) {
                                    var t = { name: "BlackBerry" },
                                        r = i.default.getFirstMatch(a, e) || i.default.getFirstMatch(/blackberry[\d]+\/(\d+(\.?_?\d+)+)/i, e);
                                    return r && (t.version = r), t;
                                },
                            },
                            {
                                test: [/(web|hpw)[o0]s/i],
                                describe: function (e) {
                                    var t = { name: "WebOS Browser" },
                                        r = i.default.getFirstMatch(a, e) || i.default.getFirstMatch(/w(?:eb)?[o0]sbrowser\/(\d+(\.?_?\d+)+)/i, e);
                                    return r && (t.version = r), t;
                                },
                            },
                            {
                                test: [/bada/i],
                                describe: function (e) {
                                    var t = { name: "Bada" },
                                        r = i.default.getFirstMatch(/dolfin\/(\d+(\.?_?\d+)+)/i, e);
                                    return r && (t.version = r), t;
                                },
                            },
                            {
                                test: [/tizen/i],
                                describe: function (e) {
                                    var t = { name: "Tizen" },
                                        r = i.default.getFirstMatch(/(?:tizen\s?)?browser\/(\d+(\.?_?\d+)+)/i, e) || i.default.getFirstMatch(a, e);
                                    return r && (t.version = r), t;
                                },
                            },
                            {
                                test: [/qupzilla/i],
                                describe: function (e) {
                                    var t = { name: "QupZilla" },
                                        r = i.default.getFirstMatch(/(?:qupzilla)[\s/](\d+(\.?_?\d+)+)/i, e) || i.default.getFirstMatch(a, e);
                                    return r && (t.version = r), t;
                                },
                            },
                            {
                                test: [/firefox|iceweasel|fxios/i],
                                describe: function (e) {
                                    var t = { name: "Firefox" },
                                        r = i.default.getFirstMatch(/(?:firefox|iceweasel|fxios)[\s/](\d+(\.?_?\d+)+)/i, e);
                                    return r && (t.version = r), t;
                                },
                            },
                            {
                                test: [/electron/i],
                                describe: function (e) {
                                    var t = { name: "Electron" },
                                        r = i.default.getFirstMatch(/(?:electron)\/(\d+(\.?_?\d+)+)/i, e);
                                    return r && (t.version = r), t;
                                },
                            },
                            {
                                test: [/chromium/i],
                                describe: function (e) {
                                    var t = { name: "Chromium" },
                                        r = i.default.getFirstMatch(/(?:chromium)[\s/](\d+(\.?_?\d+)+)/i, e) || i.default.getFirstMatch(a, e);
                                    return r && (t.version = r), t;
                                },
                            },
                            {
                                test: [/chrome|crios|crmo/i],
                                describe: function (e) {
                                    var t = { name: "Chrome" },
                                        r = i.default.getFirstMatch(/(?:chrome|crios|crmo)\/(\d+(\.?_?\d+)+)/i, e);
                                    return r && (t.version = r), t;
                                },
                            },
                            {
                                test: [/GSA/i],
                                describe: function (e) {
                                    var t = { name: "Google Search" },
                                        r = i.default.getFirstMatch(/(?:GSA)\/(\d+(\.?_?\d+)+)/i, e);
                                    return r && (t.version = r), t;
                                },
                            },
                            {
                                test: function (e) {
                                    var t = !e.test(/like android/i),
                                        r = e.test(/android/i);
                                    return t && r;
                                },
                                describe: function (e) {
                                    var t = { name: "Android Browser" },
                                        r = i.default.getFirstMatch(a, e);
                                    return r && (t.version = r), t;
                                },
                            },
                            {
                                test: [/playstation 4/i],
                                describe: function (e) {
                                    var t = { name: "PlayStation 4" },
                                        r = i.default.getFirstMatch(a, e);
                                    return r && (t.version = r), t;
                                },
                            },
                            {
                                test: [/safari|applewebkit/i],
                                describe: function (e) {
                                    var t = { name: "Safari" },
                                        r = i.default.getFirstMatch(a, e);
                                    return r && (t.version = r), t;
                                },
                            },
                            {
                                test: [/.*/i],
                                describe: function (e) {
                                    var t = -1 !== e.search("\\(") ? /^(.*)\/(.*)[ \t]\((.*)/ : /^(.*)\/(.*) /;
                                    return { name: i.default.getFirstMatch(t, e), version: i.default.getSecondMatch(t, e) };
                                },
                            },
                        ];
                    (t.default = o), (e.exports = t.default);
                },
                93: function (e, t, r) {
                    "use strict";
                    (t.__esModule = !0), (t.default = void 0);
                    var n,
                        i = (n = r(17)) && n.__esModule ? n : { default: n },
                        a = r(18),
                        o = [
                            {
                                test: [/Roku\/DVP/],
                                describe: function (e) {
                                    var t = i.default.getFirstMatch(/Roku\/DVP-(\d+\.\d+)/i, e);
                                    return { name: a.OS_MAP.Roku, version: t };
                                },
                            },
                            {
                                test: [/windows phone/i],
                                describe: function (e) {
                                    var t = i.default.getFirstMatch(/windows phone (?:os)?\s?(\d+(\.\d+)*)/i, e);
                                    return { name: a.OS_MAP.WindowsPhone, version: t };
                                },
                            },
                            {
                                test: [/windows /i],
                                describe: function (e) {
                                    var t = i.default.getFirstMatch(/Windows ((NT|XP)( \d\d?.\d)?)/i, e),
                                        r = i.default.getWindowsVersionName(t);
                                    return { name: a.OS_MAP.Windows, version: t, versionName: r };
                                },
                            },
                            {
                                test: [/Macintosh(.*?) FxiOS(.*?) Version\//],
                                describe: function (e) {
                                    var t = i.default.getSecondMatch(/(Version\/)(\d[\d.]+)/, e);
                                    return { name: a.OS_MAP.iOS, version: t };
                                },
                            },
                            {
                                test: [/macintosh/i],
                                describe: function (e) {
                                    var t = i.default.getFirstMatch(/mac os x (\d+(\.?_?\d+)+)/i, e).replace(/[_\s]/g, "."),
                                        r = i.default.getMacOSVersionName(t),
                                        n = { name: a.OS_MAP.MacOS, version: t };
                                    return r && (n.versionName = r), n;
                                },
                            },
                            {
                                test: [/(ipod|iphone|ipad)/i],
                                describe: function (e) {
                                    var t = i.default.getFirstMatch(/os (\d+([_\s]\d+)*) like mac os x/i, e).replace(/[_\s]/g, ".");
                                    return { name: a.OS_MAP.iOS, version: t };
                                },
                            },
                            {
                                test: function (e) {
                                    var t = !e.test(/like android/i),
                                        r = e.test(/android/i);
                                    return t && r;
                                },
                                describe: function (e) {
                                    var t = i.default.getFirstMatch(/android[\s/-](\d+(\.\d+)*)/i, e),
                                        r = i.default.getAndroidVersionName(t),
                                        n = { name: a.OS_MAP.Android, version: t };
                                    return r && (n.versionName = r), n;
                                },
                            },
                            {
                                test: [/(web|hpw)[o0]s/i],
                                describe: function (e) {
                                    var t = i.default.getFirstMatch(/(?:web|hpw)[o0]s\/(\d+(\.\d+)*)/i, e),
                                        r = { name: a.OS_MAP.WebOS };
                                    return t && t.length && (r.version = t), r;
                                },
                            },
                            {
                                test: [/blackberry|\bbb\d+/i, /rim\stablet/i],
                                describe: function (e) {
                                    var t = i.default.getFirstMatch(/rim\stablet\sos\s(\d+(\.\d+)*)/i, e) || i.default.getFirstMatch(/blackberry\d+\/(\d+([_\s]\d+)*)/i, e) || i.default.getFirstMatch(/\bbb(\d+)/i, e);
                                    return { name: a.OS_MAP.BlackBerry, version: t };
                                },
                            },
                            {
                                test: [/bada/i],
                                describe: function (e) {
                                    var t = i.default.getFirstMatch(/bada\/(\d+(\.\d+)*)/i, e);
                                    return { name: a.OS_MAP.Bada, version: t };
                                },
                            },
                            {
                                test: [/tizen/i],
                                describe: function (e) {
                                    var t = i.default.getFirstMatch(/tizen[/\s](\d+(\.\d+)*)/i, e);
                                    return { name: a.OS_MAP.Tizen, version: t };
                                },
                            },
                            {
                                test: [/linux/i],
                                describe: function () {
                                    return { name: a.OS_MAP.Linux };
                                },
                            },
                            {
                                test: [/CrOS/],
                                describe: function () {
                                    return { name: a.OS_MAP.ChromeOS };
                                },
                            },
                            {
                                test: [/PlayStation 4/],
                                describe: function (e) {
                                    var t = i.default.getFirstMatch(/PlayStation 4[/\s](\d+(\.\d+)*)/i, e);
                                    return { name: a.OS_MAP.PlayStation4, version: t };
                                },
                            },
                        ];
                    (t.default = o), (e.exports = t.default);
                },
                94: function (e, t, r) {
                    "use strict";
                    (t.__esModule = !0), (t.default = void 0);
                    var n,
                        i = (n = r(17)) && n.__esModule ? n : { default: n },
                        a = r(18),
                        o = [
                            {
                                test: [/googlebot/i],
                                describe: function () {
                                    return { type: "bot", vendor: "Google" };
                                },
                            },
                            {
                                test: [/huawei/i],
                                describe: function (e) {
                                    var t = i.default.getFirstMatch(/(can-l01)/i, e) && "Nova",
                                        r = { type: a.PLATFORMS_MAP.mobile, vendor: "Huawei" };
                                    return t && (r.model = t), r;
                                },
                            },
                            {
                                test: [/nexus\s*(?:7|8|9|10).*/i],
                                describe: function () {
                                    return { type: a.PLATFORMS_MAP.tablet, vendor: "Nexus" };
                                },
                            },
                            {
                                test: [/ipad/i],
                                describe: function () {
                                    return { type: a.PLATFORMS_MAP.tablet, vendor: "Apple", model: "iPad" };
                                },
                            },
                            {
                                test: [/Macintosh(.*?) FxiOS(.*?) Version\//],
                                describe: function () {
                                    return { type: a.PLATFORMS_MAP.tablet, vendor: "Apple", model: "iPad" };
                                },
                            },
                            {
                                test: [/kftt build/i],
                                describe: function () {
                                    return { type: a.PLATFORMS_MAP.tablet, vendor: "Amazon", model: "Kindle Fire HD 7" };
                                },
                            },
                            {
                                test: [/silk/i],
                                describe: function () {
                                    return { type: a.PLATFORMS_MAP.tablet, vendor: "Amazon" };
                                },
                            },
                            {
                                test: [/tablet(?! pc)/i],
                                describe: function () {
                                    return { type: a.PLATFORMS_MAP.tablet };
                                },
                            },
                            {
                                test: function (e) {
                                    var t = e.test(/ipod|iphone/i),
                                        r = e.test(/like (ipod|iphone)/i);
                                    return t && !r;
                                },
                                describe: function (e) {
                                    var t = i.default.getFirstMatch(/(ipod|iphone)/i, e);
                                    return { type: a.PLATFORMS_MAP.mobile, vendor: "Apple", model: t };
                                },
                            },
                            {
                                test: [/nexus\s*[0-6].*/i, /galaxy nexus/i],
                                describe: function () {
                                    return { type: a.PLATFORMS_MAP.mobile, vendor: "Nexus" };
                                },
                            },
                            {
                                test: [/[^-]mobi/i],
                                describe: function () {
                                    return { type: a.PLATFORMS_MAP.mobile };
                                },
                            },
                            {
                                test: function (e) {
                                    return "blackberry" === e.getBrowserName(!0);
                                },
                                describe: function () {
                                    return { type: a.PLATFORMS_MAP.mobile, vendor: "BlackBerry" };
                                },
                            },
                            {
                                test: function (e) {
                                    return "bada" === e.getBrowserName(!0);
                                },
                                describe: function () {
                                    return { type: a.PLATFORMS_MAP.mobile };
                                },
                            },
                            {
                                test: function (e) {
                                    return "windows phone" === e.getBrowserName();
                                },
                                describe: function () {
                                    return { type: a.PLATFORMS_MAP.mobile, vendor: "Microsoft" };
                                },
                            },
                            {
                                test: function (e) {
                                    var t = Number(String(e.getOSVersion()).split(".")[0]);
                                    return "android" === e.getOSName(!0) && t >= 3;
                                },
                                describe: function () {
                                    return { type: a.PLATFORMS_MAP.tablet };
                                },
                            },
                            {
                                test: function (e) {
                                    return "android" === e.getOSName(!0);
                                },
                                describe: function () {
                                    return { type: a.PLATFORMS_MAP.mobile };
                                },
                            },
                            {
                                test: function (e) {
                                    return "macos" === e.getOSName(!0);
                                },
                                describe: function () {
                                    return { type: a.PLATFORMS_MAP.desktop, vendor: "Apple" };
                                },
                            },
                            {
                                test: function (e) {
                                    return "windows" === e.getOSName(!0);
                                },
                                describe: function () {
                                    return { type: a.PLATFORMS_MAP.desktop };
                                },
                            },
                            {
                                test: function (e) {
                                    return "linux" === e.getOSName(!0);
                                },
                                describe: function () {
                                    return { type: a.PLATFORMS_MAP.desktop };
                                },
                            },
                            {
                                test: function (e) {
                                    return "playstation 4" === e.getOSName(!0);
                                },
                                describe: function () {
                                    return { type: a.PLATFORMS_MAP.tv };
                                },
                            },
                            {
                                test: function (e) {
                                    return "roku" === e.getOSName(!0);
                                },
                                describe: function () {
                                    return { type: a.PLATFORMS_MAP.tv };
                                },
                            },
                        ];
                    (t.default = o), (e.exports = t.default);
                },
                95: function (e, t, r) {
                    "use strict";
                    (t.__esModule = !0), (t.default = void 0);
                    var n,
                        i = (n = r(17)) && n.__esModule ? n : { default: n },
                        a = r(18),
                        o = [
                            {
                                test: function (e) {
                                    return "microsoft edge" === e.getBrowserName(!0);
                                },
                                describe: function (e) {
                                    if (/\sedg\//i.test(e)) return { name: a.ENGINE_MAP.Blink };
                                    var t = i.default.getFirstMatch(/edge\/(\d+(\.?_?\d+)+)/i, e);
                                    return { name: a.ENGINE_MAP.EdgeHTML, version: t };
                                },
                            },
                            {
                                test: [/trident/i],
                                describe: function (e) {
                                    var t = { name: a.ENGINE_MAP.Trident },
                                        r = i.default.getFirstMatch(/trident\/(\d+(\.?_?\d+)+)/i, e);
                                    return r && (t.version = r), t;
                                },
                            },
                            {
                                test: function (e) {
                                    return e.test(/presto/i);
                                },
                                describe: function (e) {
                                    var t = { name: a.ENGINE_MAP.Presto },
                                        r = i.default.getFirstMatch(/presto\/(\d+(\.?_?\d+)+)/i, e);
                                    return r && (t.version = r), t;
                                },
                            },
                            {
                                test: function (e) {
                                    var t = e.test(/gecko/i),
                                        r = e.test(/like gecko/i);
                                    return t && !r;
                                },
                                describe: function (e) {
                                    var t = { name: a.ENGINE_MAP.Gecko },
                                        r = i.default.getFirstMatch(/gecko\/(\d+(\.?_?\d+)+)/i, e);
                                    return r && (t.version = r), t;
                                },
                            },
                            {
                                test: [/(apple)?webkit\/537\.36/i],
                                describe: function () {
                                    return { name: a.ENGINE_MAP.Blink };
                                },
                            },
                            {
                                test: [/(apple)?webkit/i],
                                describe: function (e) {
                                    var t = { name: a.ENGINE_MAP.WebKit },
                                        r = i.default.getFirstMatch(/webkit\/(\d+(\.?_?\d+)+)/i, e);
                                    return r && (t.version = r), t;
                                },
                            },
                        ];
                    (t.default = o), (e.exports = t.default);
                },
            });
        },
        function (e, t) {
            e.exports = function (e, t) {
                for (var r = -1, n = null == e ? 0 : e.length, i = 0, a = []; ++r < n;) {
                    var o = e[r];
                    t(o, r, e) && (a[i++] = o);
                }
                return a;
            };
        },
        function (e, t, r) {
            var n = r(66),
                i = r(81)(n);
            e.exports = i;
        },
        function (e, t, r) {
            var n = r(71),
                i = r(15),
                a = Object.prototype,
                o = a.hasOwnProperty,
                s = a.propertyIsEnumerable,
                c = n(
                    (function () {
                        return arguments;
                    })()
                )
                    ? n
                    : function (e) {
                        return i(e) && o.call(e, "callee") && !s.call(e, "callee");
                    };
            e.exports = c;
        },
        function (e, t, r) {
            (function (t) {
                var r = "object" == typeof t && t && t.Object === Object && t;
                e.exports = r;
            }.call(this, r(41)));
        },
        function (e, t) {
            var r;
            r = (function () {
                return this;
            })();
            try {
                r = r || new Function("return this")();
            } catch (e) {
                "object" == typeof window && (r = window);
            }
            e.exports = r;
        },
        function (e, t, r) {
            (function (e) {
                var n = r(7),
                    i = r(74),
                    a = t && !t.nodeType && t,
                    o = a && "object" == typeof e && e && !e.nodeType && e,
                    s = o && o.exports === a ? n.Buffer : void 0,
                    c = (s ? s.isBuffer : void 0) || i;
                e.exports = c;
            }.call(this, r(43)(e)));
        },
        function (e, t) {
            e.exports = function (e) {
                return (
                    e.webpackPolyfill ||
                    ((e.deprecate = function () { }),
                        (e.paths = []),
                        e.children || (e.children = []),
                        Object.defineProperty(e, "loaded", {
                            enumerable: !0,
                            get: function () {
                                return e.l;
                            },
                        }),
                        Object.defineProperty(e, "id", {
                            enumerable: !0,
                            get: function () {
                                return e.i;
                            },
                        }),
                        (e.webpackPolyfill = 1)),
                    e
                );
            };
        },
        function (e, t) {
            var r = /^(?:0|[1-9]\d*)$/;
            e.exports = function (e, t) {
                var n = typeof e;
                return !!(t = null == t ? 9007199254740991 : t) && ("number" == n || ("symbol" != n && r.test(e))) && e > -1 && e % 1 == 0 && e < t;
            };
        },
        function (e, t, r) {
            var n = r(75),
                i = r(46),
                a = r(76),
                o = a && a.isTypedArray,
                s = o ? i(o) : n;
            e.exports = s;
        },
        function (e, t) {
            e.exports = function (e) {
                return function (t) {
                    return e(t);
                };
            };
        },
        function (e, t, r) {
            var n = r(14),
                i = r(32);
            e.exports = function (e) {
                if (!i(e)) return !1;
                var t = n(e);
                return "[object Function]" == t || "[object GeneratorFunction]" == t || "[object AsyncFunction]" == t || "[object Proxy]" == t;
            };
        },
        function (e, t, r) {
            var n = r(82),
                i = r(132),
                a = r(59),
                o = r(6),
                s = r(142);
            e.exports = function (e) {
                return "function" == typeof e ? e : null == e ? a : "object" == typeof e ? (o(e) ? i(e[0], e[1]) : n(e)) : s(e);
            };
        },
        function (e, t, r) {
            var n = r(21),
                i = r(89),
                a = r(90),
                o = r(91),
                s = r(92),
                c = r(93);
            function u(e) {
                var t = (this.__data__ = new n(e));
                this.size = t.size;
            }
            (u.prototype.clear = i), (u.prototype.delete = a), (u.prototype.get = o), (u.prototype.has = s), (u.prototype.set = c), (e.exports = u);
        },
        function (e, t) {
            e.exports = function (e, t) {
                return e === t || (e != e && t != t);
            };
        },
        function (e, t) {
            var r = Function.prototype.toString;
            e.exports = function (e) {
                if (null != e) {
                    try {
                        return r.call(e);
                    } catch (e) { }
                    try {
                        return e + "";
                    } catch (e) { }
                }
                return "";
            };
        },
        function (e, t, r) {
            var n = r(110),
                i = r(15);
            e.exports = function e(t, r, a, o, s) {
                return t === r || (null == t || null == r || (!i(t) && !i(r)) ? t != t && r != r : n(t, r, a, o, e, s));
            };
        },
        function (e, t, r) {
            var n = r(111),
                i = r(114),
                a = r(115);
            e.exports = function (e, t, r, o, s, c) {
                var u = 1 & r,
                    l = e.length,
                    d = t.length;
                if (l != d && !(u && d > l)) return !1;
                var f = c.get(e);
                if (f && c.get(t)) return f == t;
                var p = -1,
                    h = !0,
                    v = 2 & r ? new n() : void 0;
                for (c.set(e, t), c.set(t, e); ++p < l;) {
                    var m = e[p],
                        g = t[p];
                    if (o) var b = u ? o(g, m, p, t, e, c) : o(m, g, p, e, t, c);
                    if (void 0 !== b) {
                        if (b) continue;
                        h = !1;
                        break;
                    }
                    if (v) {
                        if (
                            !i(t, function (e, t) {
                                if (!a(v, t) && (m === e || s(m, e, r, o, c))) return v.push(t);
                            })
                        ) {
                            h = !1;
                            break;
                        }
                    } else if (m !== g && !s(m, g, r, o, c)) {
                        h = !1;
                        break;
                    }
                }
                return c.delete(e), c.delete(t), h;
            };
        },
        function (e, t, r) {
            var n = r(32);
            e.exports = function (e) {
                return e == e && !n(e);
            };
        },
        function (e, t) {
            e.exports = function (e, t) {
                return function (r) {
                    return null != r && r[e] === t && (void 0 !== t || e in Object(r));
                };
            };
        },
        function (e, t, r) {
            var n = r(57),
                i = r(26);
            e.exports = function (e, t) {
                for (var r = 0, a = (t = n(t, e)).length; null != e && r < a;) e = e[i(t[r++])];
                return r && r == a ? e : void 0;
            };
        },
        function (e, t, r) {
            var n = r(6),
                i = r(35),
                a = r(134),
                o = r(137);
            e.exports = function (e, t) {
                return n(e) ? e : i(e, t) ? [e] : a(o(e));
            };
        },
        function (e, t) {
            e.exports = function (e, t) {
                for (var r = -1, n = null == e ? 0 : e.length, i = Array(n); ++r < n;) i[r] = t(e[r], r, e);
                return i;
            };
        },
        function (e, t) {
            e.exports = function (e) {
                return e;
            };
        },
        function (e, t, r) {
            "use strict";
            (function (e) {
                r.d(t, "a", function () {
                    return v;
                });
                var n = r(3),
                    i = r.n(n),
                    a = r(4),
                    o = r.n(a),
                    s = r(9),
                    c = r.n(s),
                    u = r(8),
                    l = r.n(u),
                    d = r(10),
                    f = r.n(d),
                    p = r(27),
                    h = r(11);
                (e.callMachineToDailyJsEmitter = e.callMachineToDailyJsEmitter || new h.EventEmitter()), (e.dailyJsToCallMachineEmitter = e.dailyJsToCallMachineEmitter || new h.EventEmitter());
                var v = (function (t) {
                    function r() {
                        var e;
                        return i()(this, r), ((e = c()(this, l()(r).call(this)))._wrappedListeners = {}), (e._messageCallbacks = {}), e;
                    }
                    return (
                        f()(r, t),
                        o()(r, [
                            {
                                key: "addListenerForMessagesFromCallMachine",
                                value: function (t, r, n) {
                                    this._addListener(t, e.callMachineToDailyJsEmitter, n, "received call machine message");
                                },
                            },
                            {
                                key: "addListenerForMessagesFromDailyJs",
                                value: function (t, r, n) {
                                    this._addListener(t, e.dailyJsToCallMachineEmitter, n, "received daily-js message");
                                },
                            },
                            {
                                key: "sendMessageToCallMachine",
                                value: function (t, r) {
                                    this._sendMessage(t, e.dailyJsToCallMachineEmitter, "sending message to call machine", r);
                                },
                            },
                            {
                                key: "sendMessageToDailyJs",
                                value: function (t) {
                                    this._sendMessage(t, e.callMachineToDailyJsEmitter, "sending message to daily-js");
                                },
                            },
                            {
                                key: "removeListener",
                                value: function (t) {
                                    var r = this._wrappedListeners[t];
                                    r && (e.callMachineToDailyJsEmitter.removeListener("message", r), e.dailyJsToCallMachineEmitter.removeListener("message", r), delete this._wrappedListeners[t]);
                                },
                            },
                            {
                                key: "_addListener",
                                value: function (e, t, r, n) {
                                    var i = this,
                                        a = function (t) {
                                            if (t.callbackStamp && i._messageCallbacks[t.callbackStamp]) {
                                                var n = t.callbackStamp;
                                                i._messageCallbacks[n].call(r, t), delete i._messageCallbacks[n];
                                            }
                                            e.call(r, t);
                                        };
                                    (this._wrappedListeners[e] = a), t.addListener("message", a);
                                },
                            },
                            {
                                key: "_sendMessage",
                                value: function (e, t, r, n) {
                                    if (n) {
                                        var i = Date.now();
                                        (this._messageCallbacks[i] = n), (e.callbackStamp = i);
                                    }
                                    t.emit("message", e);
                                },
                            },
                        ]),
                        r
                    );
                })(p.a);
            }.call(this, r(41)));
        },
        function (e, t, r) {
            var n = r(8),
                i = r(28),
                a = r(150),
                o = r(151);
            function s(t) {
                var r = "function" == typeof Map ? new Map() : void 0;
                return (
                    (e.exports = s = function (e) {
                        if (null === e || !a(e)) return e;
                        if ("function" != typeof e) throw new TypeError("Super expression must either be null or a function");
                        if (void 0 !== r) {
                            if (r.has(e)) return r.get(e);
                            r.set(e, t);
                        }
                        function t() {
                            return o(e, arguments, n(this).constructor);
                        }
                        return (t.prototype = Object.create(e.prototype, { constructor: { value: t, enumerable: !1, writable: !0, configurable: !0 } })), i(t, e);
                    }),
                    s(t)
                );
            }
            e.exports = s;
        },
        function (e, t, r) {
            !(function (e) {
                "use strict";
                var t = "function" == typeof Map,
                    r = "function" == typeof Set,
                    n = "function" == typeof WeakSet,
                    i = Object.keys,
                    a = function (e, t) {
                        return e && "object" == typeof e && t.add(e);
                    },
                    o = function (e, t, r, n) {
                        for (var i, a = 0; a < e.length; a++) if (r((i = e[a])[0], t[0], n) && r(i[1], t[1], n)) return !0;
                        return !1;
                    },
                    s = function (e, t, r, n) {
                        for (var i = 0; i < e.length; i++) if (r(e[i], t, n)) return !0;
                        return !1;
                    },
                    c = function (e, t) {
                        return e === t || (e != e && t != t);
                    },
                    u = function (e) {
                        return e.constructor === Object;
                    },
                    l = function (e) {
                        return "function" == typeof e.then;
                    },
                    d = function (e) {
                        return !(!e.$$typeof || !e._store);
                    },
                    f = function (e) {
                        return function (t) {
                            var r = e || t;
                            return function (e, t, i) {
                                void 0 === i &&
                                    (i = n
                                        ? new WeakSet()
                                        : Object.create({
                                            _values: [],
                                            add: function (e) {
                                                this._values.push(e);
                                            },
                                            has: function (e) {
                                                return !!~this._values.indexOf(e);
                                            },
                                        }));
                                var o = i.has(e),
                                    s = i.has(t);
                                return o || s ? o && s : (a(e, i), a(t, i), r(e, t, i));
                            };
                        };
                    },
                    p = function (e) {
                        var t = [];
                        return (
                            e.forEach(function (e, r) {
                                return t.push([r, e]);
                            }),
                            t
                        );
                    },
                    h = function (e) {
                        var t = [];
                        return (
                            e.forEach(function (e) {
                                return t.push(e);
                            }),
                            t
                        );
                    },
                    v = function (e, t, r, n) {
                        var a,
                            o = i(e),
                            u = i(t);
                        if (o.length !== u.length) return !1;
                        for (var l = 0; l < o.length; l++) {
                            if (((a = o[l]), !s(u, a, c))) return !1;
                            if (!(("_owner" === a && d(e) && d(t)) || r(e[a], t[a], n))) return !1;
                        }
                        return !0;
                    },
                    m = Array.isArray,
                    g = function (e) {
                        var n = "function" == typeof e ? e(i) : i;
                        function i(e, i, a) {
                            if (c(e, i)) return !0;
                            var d = typeof e;
                            if (d !== typeof i || "object" !== d || !e || !i) return !1;
                            if (u(e) && u(i)) return v(e, i, n, a);
                            var f = m(e),
                                g = m(i);
                            if (f || g)
                                return (
                                    f === g &&
                                    (function (e, t, r, n) {
                                        if (e.length !== t.length) return !1;
                                        for (var i = 0; i < e.length; i++) if (!r(e[i], t[i], n)) return !1;
                                        return !0;
                                    })(e, i, n, a)
                                );
                            var b = e instanceof Date,
                                y = i instanceof Date;
                            if (b || y) return b === y && c(e.getTime(), i.getTime());
                            var _,
                                k,
                                w = e instanceof RegExp,
                                M = i instanceof RegExp;
                            if (w || M)
                                return (
                                    w === M &&
                                    ((k = i),
                                        (_ = e).source === k.source && _.global === k.global && _.ignoreCase === k.ignoreCase && _.multiline === k.multiline && _.unicode === k.unicode && _.sticky === k.sticky && _.lastIndex === k.lastIndex)
                                );
                            if (l(e) || l(i)) return e === i;
                            if (t) {
                                var S = e instanceof Map,
                                    x = i instanceof Map;
                                if (S || x)
                                    return (
                                        S === x &&
                                        (function (e, t, r, n) {
                                            if (e.size !== t.size) return !1;
                                            for (var i = p(e), a = p(t), s = 0; s < i.length; s++) if (!o(a, i[s], r, n) || !o(i, a[s], r, n)) return !1;
                                            return !0;
                                        })(e, i, n, a)
                                    );
                            }
                            if (r) {
                                var O = e instanceof Set,
                                    T = i instanceof Set;
                                if (O || T)
                                    return (
                                        O === T &&
                                        (function (e, t, r, n) {
                                            if (e.size !== t.size) return !1;
                                            for (var i = h(e), a = h(t), o = 0; o < i.length; o++) if (!s(a, i[o], r, n) || !s(i, a[o], r, n)) return !1;
                                            return !0;
                                        })(e, i, n, a)
                                    );
                            }
                            return v(e, i, n, a);
                        }
                        return i;
                    },
                    b = g(f()),
                    y = g(f(c)),
                    _ = g(),
                    k = g(function () {
                        return c;
                    }),
                    w = { circularDeep: b, circularShallow: y, createCustom: g, deep: _, sameValueZero: c, shallow: k };
                (e.circularDeepEqual = b), (e.circularShallowEqual = y), (e.createCustomEqual = g), (e.deepEqual = _), (e.default = w), (e.sameValueZeroEqual = c), (e.shallowEqual = k), Object.defineProperty(e, "__esModule", { value: !0 });
            })(t);
        },
        function (e, t, r) {
            var n = r(152);
            e.exports = n.default;
        },
        function (e, t, r) {
            var n = (function (e) {
                "use strict";
                var t = Object.prototype,
                    r = t.hasOwnProperty,
                    n = "function" == typeof Symbol ? Symbol : {},
                    i = n.iterator || "@@iterator",
                    a = n.asyncIterator || "@@asyncIterator",
                    o = n.toStringTag || "@@toStringTag";
                function s(e, t, r, n) {
                    var i = t && t.prototype instanceof l ? t : l,
                        a = Object.create(i.prototype),
                        o = new w(n || []);
                    return (
                        (a._invoke = (function (e, t, r) {
                            var n = "suspendedStart";
                            return function (i, a) {
                                if ("executing" === n) throw new Error("Generator is already running");
                                if ("completed" === n) {
                                    if ("throw" === i) throw a;
                                    return S();
                                }
                                for (r.method = i, r.arg = a; ;) {
                                    var o = r.delegate;
                                    if (o) {
                                        var s = y(o, r);
                                        if (s) {
                                            if (s === u) continue;
                                            return s;
                                        }
                                    }
                                    if ("next" === r.method) r.sent = r._sent = r.arg;
                                    else if ("throw" === r.method) {
                                        if ("suspendedStart" === n) throw ((n = "completed"), r.arg);
                                        r.dispatchException(r.arg);
                                    } else "return" === r.method && r.abrupt("return", r.arg);
                                    n = "executing";
                                    var l = c(e, t, r);
                                    if ("normal" === l.type) {
                                        if (((n = r.done ? "completed" : "suspendedYield"), l.arg === u)) continue;
                                        return { value: l.arg, done: r.done };
                                    }
                                    "throw" === l.type && ((n = "completed"), (r.method = "throw"), (r.arg = l.arg));
                                }
                            };
                        })(e, r, o)),
                        a
                    );
                }
                function c(e, t, r) {
                    try {
                        return { type: "normal", arg: e.call(t, r) };
                    } catch (e) {
                        return { type: "throw", arg: e };
                    }
                }
                e.wrap = s;
                var u = {};
                function l() { }
                function d() { }
                function f() { }
                var p = {};
                p[i] = function () {
                    return this;
                };
                var h = Object.getPrototypeOf,
                    v = h && h(h(M([])));
                v && v !== t && r.call(v, i) && (p = v);
                var m = (f.prototype = l.prototype = Object.create(p));
                function g(e) {
                    ["next", "throw", "return"].forEach(function (t) {
                        e[t] = function (e) {
                            return this._invoke(t, e);
                        };
                    });
                }
                function b(e) {
                    var t;
                    this._invoke = function (n, i) {
                        function a() {
                            return new Promise(function (t, a) {
                                !(function t(n, i, a, o) {
                                    var s = c(e[n], e, i);
                                    if ("throw" !== s.type) {
                                        var u = s.arg,
                                            l = u.value;
                                        return l && "object" == typeof l && r.call(l, "__await")
                                            ? Promise.resolve(l.__await).then(
                                                function (e) {
                                                    t("next", e, a, o);
                                                },
                                                function (e) {
                                                    t("throw", e, a, o);
                                                }
                                            )
                                            : Promise.resolve(l).then(
                                                function (e) {
                                                    (u.value = e), a(u);
                                                },
                                                function (e) {
                                                    return t("throw", e, a, o);
                                                }
                                            );
                                    }
                                    o(s.arg);
                                })(n, i, t, a);
                            });
                        }
                        return (t = t ? t.then(a, a) : a());
                    };
                }
                function y(e, t) {
                    var r = e.iterator[t.method];
                    if (void 0 === r) {
                        if (((t.delegate = null), "throw" === t.method)) {
                            if (e.iterator.return && ((t.method = "return"), (t.arg = void 0), y(e, t), "throw" === t.method)) return u;
                            (t.method = "throw"), (t.arg = new TypeError("The iterator does not provide a 'throw' method"));
                        }
                        return u;
                    }
                    var n = c(r, e.iterator, t.arg);
                    if ("throw" === n.type) return (t.method = "throw"), (t.arg = n.arg), (t.delegate = null), u;
                    var i = n.arg;
                    return i
                        ? i.done
                            ? ((t[e.resultName] = i.value), (t.next = e.nextLoc), "return" !== t.method && ((t.method = "next"), (t.arg = void 0)), (t.delegate = null), u)
                            : i
                        : ((t.method = "throw"), (t.arg = new TypeError("iterator result is not an object")), (t.delegate = null), u);
                }
                function _(e) {
                    var t = { tryLoc: e[0] };
                    1 in e && (t.catchLoc = e[1]), 2 in e && ((t.finallyLoc = e[2]), (t.afterLoc = e[3])), this.tryEntries.push(t);
                }
                function k(e) {
                    var t = e.completion || {};
                    (t.type = "normal"), delete t.arg, (e.completion = t);
                }
                function w(e) {
                    (this.tryEntries = [{ tryLoc: "root" }]), e.forEach(_, this), this.reset(!0);
                }
                function M(e) {
                    if (e) {
                        var t = e[i];
                        if (t) return t.call(e);
                        if ("function" == typeof e.next) return e;
                        if (!isNaN(e.length)) {
                            var n = -1,
                                a = function t() {
                                    for (; ++n < e.length;) if (r.call(e, n)) return (t.value = e[n]), (t.done = !1), t;
                                    return (t.value = void 0), (t.done = !0), t;
                                };
                            return (a.next = a);
                        }
                    }
                    return { next: S };
                }
                function S() {
                    return { value: void 0, done: !0 };
                }
                return (
                    (d.prototype = m.constructor = f),
                    (f.constructor = d),
                    (f[o] = d.displayName = "GeneratorFunction"),
                    (e.isGeneratorFunction = function (e) {
                        var t = "function" == typeof e && e.constructor;
                        return !!t && (t === d || "GeneratorFunction" === (t.displayName || t.name));
                    }),
                    (e.mark = function (e) {
                        return Object.setPrototypeOf ? Object.setPrototypeOf(e, f) : ((e.__proto__ = f), o in e || (e[o] = "GeneratorFunction")), (e.prototype = Object.create(m)), e;
                    }),
                    (e.awrap = function (e) {
                        return { __await: e };
                    }),
                    g(b.prototype),
                    (b.prototype[a] = function () {
                        return this;
                    }),
                    (e.AsyncIterator = b),
                    (e.async = function (t, r, n, i) {
                        var a = new b(s(t, r, n, i));
                        return e.isGeneratorFunction(r)
                            ? a
                            : a.next().then(function (e) {
                                return e.done ? e.value : a.next();
                            });
                    }),
                    g(m),
                    (m[o] = "Generator"),
                    (m[i] = function () {
                        return this;
                    }),
                    (m.toString = function () {
                        return "[object Generator]";
                    }),
                    (e.keys = function (e) {
                        var t = [];
                        for (var r in e) t.push(r);
                        return (
                            t.reverse(),
                            function r() {
                                for (; t.length;) {
                                    var n = t.pop();
                                    if (n in e) return (r.value = n), (r.done = !1), r;
                                }
                                return (r.done = !0), r;
                            }
                        );
                    }),
                    (e.values = M),
                    (w.prototype = {
                        constructor: w,
                        reset: function (e) {
                            if (((this.prev = 0), (this.next = 0), (this.sent = this._sent = void 0), (this.done = !1), (this.delegate = null), (this.method = "next"), (this.arg = void 0), this.tryEntries.forEach(k), !e))
                                for (var t in this) "t" === t.charAt(0) && r.call(this, t) && !isNaN(+t.slice(1)) && (this[t] = void 0);
                        },
                        stop: function () {
                            this.done = !0;
                            var e = this.tryEntries[0].completion;
                            if ("throw" === e.type) throw e.arg;
                            return this.rval;
                        },
                        dispatchException: function (e) {
                            if (this.done) throw e;
                            var t = this;
                            function n(r, n) {
                                return (o.type = "throw"), (o.arg = e), (t.next = r), n && ((t.method = "next"), (t.arg = void 0)), !!n;
                            }
                            for (var i = this.tryEntries.length - 1; i >= 0; --i) {
                                var a = this.tryEntries[i],
                                    o = a.completion;
                                if ("root" === a.tryLoc) return n("end");
                                if (a.tryLoc <= this.prev) {
                                    var s = r.call(a, "catchLoc"),
                                        c = r.call(a, "finallyLoc");
                                    if (s && c) {
                                        if (this.prev < a.catchLoc) return n(a.catchLoc, !0);
                                        if (this.prev < a.finallyLoc) return n(a.finallyLoc);
                                    } else if (s) {
                                        if (this.prev < a.catchLoc) return n(a.catchLoc, !0);
                                    } else {
                                        if (!c) throw new Error("try statement without catch or finally");
                                        if (this.prev < a.finallyLoc) return n(a.finallyLoc);
                                    }
                                }
                            }
                        },
                        abrupt: function (e, t) {
                            for (var n = this.tryEntries.length - 1; n >= 0; --n) {
                                var i = this.tryEntries[n];
                                if (i.tryLoc <= this.prev && r.call(i, "finallyLoc") && this.prev < i.finallyLoc) {
                                    var a = i;
                                    break;
                                }
                            }
                            a && ("break" === e || "continue" === e) && a.tryLoc <= t && t <= a.finallyLoc && (a = null);
                            var o = a ? a.completion : {};
                            return (o.type = e), (o.arg = t), a ? ((this.method = "next"), (this.next = a.finallyLoc), u) : this.complete(o);
                        },
                        complete: function (e, t) {
                            if ("throw" === e.type) throw e.arg;
                            return (
                                "break" === e.type || "continue" === e.type
                                    ? (this.next = e.arg)
                                    : "return" === e.type
                                        ? ((this.rval = this.arg = e.arg), (this.method = "return"), (this.next = "end"))
                                        : "normal" === e.type && t && (this.next = t),
                                u
                            );
                        },
                        finish: function (e) {
                            for (var t = this.tryEntries.length - 1; t >= 0; --t) {
                                var r = this.tryEntries[t];
                                if (r.finallyLoc === e) return this.complete(r.completion, r.afterLoc), k(r), u;
                            }
                        },
                        catch: function (e) {
                            for (var t = this.tryEntries.length - 1; t >= 0; --t) {
                                var r = this.tryEntries[t];
                                if (r.tryLoc === e) {
                                    var n = r.completion;
                                    if ("throw" === n.type) {
                                        var i = n.arg;
                                        k(r);
                                    }
                                    return i;
                                }
                            }
                            throw new Error("illegal catch attempt");
                        },
                        delegateYield: function (e, t, r) {
                            return (this.delegate = { iterator: M(e), resultName: t, nextLoc: r }), "next" === this.method && (this.arg = void 0), u;
                        },
                    }),
                    e
                );
            })(e.exports);
            try {
                regeneratorRuntime = n;
            } catch (e) {
                Function("r", "regeneratorRuntime = r")(n);
            }
        },
        function (e, t, r) {
            var n = r(38);
            e.exports = function (e, t) {
                var r = [];
                return (
                    n(e, function (e, n, i) {
                        t(e, n, i) && r.push(e);
                    }),
                    r
                );
            };
        },
        function (e, t, r) {
            var n = r(67),
                i = r(29);
            e.exports = function (e, t) {
                return e && n(e, t, i);
            };
        },
        function (e, t, r) {
            var n = r(68)();
            e.exports = n;
        },
        function (e, t) {
            e.exports = function (e) {
                return function (t, r, n) {
                    for (var i = -1, a = Object(t), o = n(t), s = o.length; s--;) {
                        var c = o[e ? s : ++i];
                        if (!1 === r(a[c], c, a)) break;
                    }
                    return t;
                };
            };
        },
        function (e, t, r) {
            var n = r(70),
                i = r(39),
                a = r(6),
                o = r(42),
                s = r(44),
                c = r(45),
                u = Object.prototype.hasOwnProperty;
            e.exports = function (e, t) {
                var r = a(e),
                    l = !r && i(e),
                    d = !r && !l && o(e),
                    f = !r && !l && !d && c(e),
                    p = r || l || d || f,
                    h = p ? n(e.length, String) : [],
                    v = h.length;
                for (var m in e) (!t && !u.call(e, m)) || (p && ("length" == m || (d && ("offset" == m || "parent" == m)) || (f && ("buffer" == m || "byteLength" == m || "byteOffset" == m)) || s(m, v))) || h.push(m);
                return h;
            };
        },
        function (e, t) {
            e.exports = function (e, t) {
                for (var r = -1, n = Array(e); ++r < e;) n[r] = t(r);
                return n;
            };
        },
        function (e, t, r) {
            var n = r(14),
                i = r(15);
            e.exports = function (e) {
                return i(e) && "[object Arguments]" == n(e);
            };
        },
        function (e, t, r) {
            var n = r(20),
                i = Object.prototype,
                a = i.hasOwnProperty,
                o = i.toString,
                s = n ? n.toStringTag : void 0;
            e.exports = function (e) {
                var t = a.call(e, s),
                    r = e[s];
                try {
                    e[s] = void 0;
                    var n = !0;
                } catch (e) { }
                var i = o.call(e);
                return n && (t ? (e[s] = r) : delete e[s]), i;
            };
        },
        function (e, t) {
            var r = Object.prototype.toString;
            e.exports = function (e) {
                return r.call(e);
            };
        },
        function (e, t) {
            e.exports = function () {
                return !1;
            };
        },
        function (e, t, r) {
            var n = r(14),
                i = r(30),
                a = r(15),
                o = {};
            (o["[object Float32Array]"] = o["[object Float64Array]"] = o["[object Int8Array]"] = o["[object Int16Array]"] = o["[object Int32Array]"] = o["[object Uint8Array]"] = o["[object Uint8ClampedArray]"] = o[
                "[object Uint16Array]"
            ] = o["[object Uint32Array]"] = !0),
                (o["[object Arguments]"] = o["[object Array]"] = o["[object ArrayBuffer]"] = o["[object Boolean]"] = o["[object DataView]"] = o["[object Date]"] = o["[object Error]"] = o["[object Function]"] = o["[object Map]"] = o[
                    "[object Number]"
                ] = o["[object Object]"] = o["[object RegExp]"] = o["[object Set]"] = o["[object String]"] = o["[object WeakMap]"] = !1),
                (e.exports = function (e) {
                    return a(e) && i(e.length) && !!o[n(e)];
                });
        },
        function (e, t, r) {
            (function (e) {
                var n = r(40),
                    i = t && !t.nodeType && t,
                    a = i && "object" == typeof e && e && !e.nodeType && e,
                    o = a && a.exports === i && n.process,
                    s = (function () {
                        try {
                            var e = a && a.require && a.require("util").types;
                            return e || (o && o.binding && o.binding("util"));
                        } catch (e) { }
                    })();
                e.exports = s;
            }.call(this, r(43)(e)));
        },
        function (e, t, r) {
            var n = r(78),
                i = r(79),
                a = Object.prototype.hasOwnProperty;
            e.exports = function (e) {
                if (!n(e)) return i(e);
                var t = [];
                for (var r in Object(e)) a.call(e, r) && "constructor" != r && t.push(r);
                return t;
            };
        },
        function (e, t) {
            var r = Object.prototype;
            e.exports = function (e) {
                var t = e && e.constructor;
                return e === (("function" == typeof t && t.prototype) || r);
            };
        },
        function (e, t, r) {
            var n = r(80)(Object.keys, Object);
            e.exports = n;
        },
        function (e, t) {
            e.exports = function (e, t) {
                return function (r) {
                    return e(t(r));
                };
            };
        },
        function (e, t, r) {
            var n = r(31);
            e.exports = function (e, t) {
                return function (r, i) {
                    if (null == r) return r;
                    if (!n(r)) return e(r, i);
                    for (var a = r.length, o = t ? a : -1, s = Object(r); (t ? o-- : ++o < a) && !1 !== i(s[o], o, s););
                    return r;
                };
            };
        },
        function (e, t, r) {
            var n = r(83),
                i = r(131),
                a = r(55);
            e.exports = function (e) {
                var t = i(e);
                return 1 == t.length && t[0][2]
                    ? a(t[0][0], t[0][1])
                    : function (r) {
                        return r === e || n(r, e, t);
                    };
            };
        },
        function (e, t, r) {
            var n = r(49),
                i = r(52);
            e.exports = function (e, t, r, a) {
                var o = r.length,
                    s = o,
                    c = !a;
                if (null == e) return !s;
                for (e = Object(e); o--;) {
                    var u = r[o];
                    if (c && u[2] ? u[1] !== e[u[0]] : !(u[0] in e)) return !1;
                }
                for (; ++o < s;) {
                    var l = (u = r[o])[0],
                        d = e[l],
                        f = u[1];
                    if (c && u[2]) {
                        if (void 0 === d && !(l in e)) return !1;
                    } else {
                        var p = new n();
                        if (a) var h = a(d, f, l, e, t, p);
                        if (!(void 0 === h ? i(f, d, 3, a, p) : h)) return !1;
                    }
                }
                return !0;
            };
        },
        function (e, t) {
            e.exports = function () {
                (this.__data__ = []), (this.size = 0);
            };
        },
        function (e, t, r) {
            var n = r(22),
                i = Array.prototype.splice;
            e.exports = function (e) {
                var t = this.__data__,
                    r = n(t, e);
                return !(r < 0) && (r == t.length - 1 ? t.pop() : i.call(t, r, 1), --this.size, !0);
            };
        },
        function (e, t, r) {
            var n = r(22);
            e.exports = function (e) {
                var t = this.__data__,
                    r = n(t, e);
                return r < 0 ? void 0 : t[r][1];
            };
        },
        function (e, t, r) {
            var n = r(22);
            e.exports = function (e) {
                return n(this.__data__, e) > -1;
            };
        },
        function (e, t, r) {
            var n = r(22);
            e.exports = function (e, t) {
                var r = this.__data__,
                    i = n(r, e);
                return i < 0 ? (++this.size, r.push([e, t])) : (r[i][1] = t), this;
            };
        },
        function (e, t, r) {
            var n = r(21);
            e.exports = function () {
                (this.__data__ = new n()), (this.size = 0);
            };
        },
        function (e, t) {
            e.exports = function (e) {
                var t = this.__data__,
                    r = t.delete(e);
                return (this.size = t.size), r;
            };
        },
        function (e, t) {
            e.exports = function (e) {
                return this.__data__.get(e);
            };
        },
        function (e, t) {
            e.exports = function (e) {
                return this.__data__.has(e);
            };
        },
        function (e, t, r) {
            var n = r(21),
                i = r(33),
                a = r(34);
            e.exports = function (e, t) {
                var r = this.__data__;
                if (r instanceof n) {
                    var o = r.__data__;
                    if (!i || o.length < 199) return o.push([e, t]), (this.size = ++r.size), this;
                    r = this.__data__ = new a(o);
                }
                return r.set(e, t), (this.size = r.size), this;
            };
        },
        function (e, t, r) {
            var n = r(47),
                i = r(95),
                a = r(32),
                o = r(51),
                s = /^\[object .+?Constructor\]$/,
                c = Function.prototype,
                u = Object.prototype,
                l = c.toString,
                d = u.hasOwnProperty,
                f = RegExp(
                    "^" +
                    l
                        .call(d)
                        .replace(/[\\^$.*+?()[\]{}|]/g, "\\$&")
                        .replace(/hasOwnProperty|(function).*?(?=\\\()| for .+?(?=\\\])/g, "$1.*?") +
                    "$"
                );
            e.exports = function (e) {
                return !(!a(e) || i(e)) && (n(e) ? f : s).test(o(e));
            };
        },
        function (e, t, r) {
            var n,
                i = r(96),
                a = (n = /[^.]+$/.exec((i && i.keys && i.keys.IE_PROTO) || "")) ? "Symbol(src)_1." + n : "";
            e.exports = function (e) {
                return !!a && a in e;
            };
        },
        function (e, t, r) {
            var n = r(7)["__core-js_shared__"];
            e.exports = n;
        },
        function (e, t) {
            e.exports = function (e, t) {
                return null == e ? void 0 : e[t];
            };
        },
        function (e, t, r) {
            var n = r(99),
                i = r(21),
                a = r(33);
            e.exports = function () {
                (this.size = 0), (this.__data__ = { hash: new n(), map: new (a || i)(), string: new n() });
            };
        },
        function (e, t, r) {
            var n = r(100),
                i = r(101),
                a = r(102),
                o = r(103),
                s = r(104);
            function c(e) {
                var t = -1,
                    r = null == e ? 0 : e.length;
                for (this.clear(); ++t < r;) {
                    var n = e[t];
                    this.set(n[0], n[1]);
                }
            }
            (c.prototype.clear = n), (c.prototype.delete = i), (c.prototype.get = a), (c.prototype.has = o), (c.prototype.set = s), (e.exports = c);
        },
        function (e, t, r) {
            var n = r(23);
            e.exports = function () {
                (this.__data__ = n ? n(null) : {}), (this.size = 0);
            };
        },
        function (e, t) {
            e.exports = function (e) {
                var t = this.has(e) && delete this.__data__[e];
                return (this.size -= t ? 1 : 0), t;
            };
        },
        function (e, t, r) {
            var n = r(23),
                i = Object.prototype.hasOwnProperty;
            e.exports = function (e) {
                var t = this.__data__;
                if (n) {
                    var r = t[e];
                    return "__lodash_hash_undefined__" === r ? void 0 : r;
                }
                return i.call(t, e) ? t[e] : void 0;
            };
        },
        function (e, t, r) {
            var n = r(23),
                i = Object.prototype.hasOwnProperty;
            e.exports = function (e) {
                var t = this.__data__;
                return n ? void 0 !== t[e] : i.call(t, e);
            };
        },
        function (e, t, r) {
            var n = r(23);
            e.exports = function (e, t) {
                var r = this.__data__;
                return (this.size += this.has(e) ? 0 : 1), (r[e] = n && void 0 === t ? "__lodash_hash_undefined__" : t), this;
            };
        },
        function (e, t, r) {
            var n = r(24);
            e.exports = function (e) {
                var t = n(this, e).delete(e);
                return (this.size -= t ? 1 : 0), t;
            };
        },
        function (e, t) {
            e.exports = function (e) {
                var t = typeof e;
                return "string" == t || "number" == t || "symbol" == t || "boolean" == t ? "__proto__" !== e : null === e;
            };
        },
        function (e, t, r) {
            var n = r(24);
            e.exports = function (e) {
                return n(this, e).get(e);
            };
        },
        function (e, t, r) {
            var n = r(24);
            e.exports = function (e) {
                return n(this, e).has(e);
            };
        },
        function (e, t, r) {
            var n = r(24);
            e.exports = function (e, t) {
                var r = n(this, e),
                    i = r.size;
                return r.set(e, t), (this.size += r.size == i ? 0 : 1), this;
            };
        },
        function (e, t, r) {
            var n = r(49),
                i = r(53),
                a = r(116),
                o = r(120),
                s = r(126),
                c = r(6),
                u = r(42),
                l = r(45),
                d = "[object Object]",
                f = Object.prototype.hasOwnProperty;
            e.exports = function (e, t, r, p, h, v) {
                var m = c(e),
                    g = c(t),
                    b = m ? "[object Array]" : s(e),
                    y = g ? "[object Array]" : s(t),
                    _ = (b = "[object Arguments]" == b ? d : b) == d,
                    k = (y = "[object Arguments]" == y ? d : y) == d,
                    w = b == y;
                if (w && u(e)) {
                    if (!u(t)) return !1;
                    (m = !0), (_ = !1);
                }
                if (w && !_) return v || (v = new n()), m || l(e) ? i(e, t, r, p, h, v) : a(e, t, b, r, p, h, v);
                if (!(1 & r)) {
                    var M = _ && f.call(e, "__wrapped__"),
                        S = k && f.call(t, "__wrapped__");
                    if (M || S) {
                        var x = M ? e.value() : e,
                            O = S ? t.value() : t;
                        return v || (v = new n()), h(x, O, r, p, v);
                    }
                }
                return !!w && (v || (v = new n()), o(e, t, r, p, h, v));
            };
        },
        function (e, t, r) {
            var n = r(34),
                i = r(112),
                a = r(113);
            function o(e) {
                var t = -1,
                    r = null == e ? 0 : e.length;
                for (this.__data__ = new n(); ++t < r;) this.add(e[t]);
            }
            (o.prototype.add = o.prototype.push = i), (o.prototype.has = a), (e.exports = o);
        },
        function (e, t) {
            e.exports = function (e) {
                return this.__data__.set(e, "__lodash_hash_undefined__"), this;
            };
        },
        function (e, t) {
            e.exports = function (e) {
                return this.__data__.has(e);
            };
        },
        function (e, t) {
            e.exports = function (e, t) {
                for (var r = -1, n = null == e ? 0 : e.length; ++r < n;) if (t(e[r], r, e)) return !0;
                return !1;
            };
        },
        function (e, t) {
            e.exports = function (e, t) {
                return e.has(t);
            };
        },
        function (e, t, r) {
            var n = r(20),
                i = r(117),
                a = r(50),
                o = r(53),
                s = r(118),
                c = r(119),
                u = n ? n.prototype : void 0,
                l = u ? u.valueOf : void 0;
            e.exports = function (e, t, r, n, u, d, f) {
                switch (r) {
                    case "[object DataView]":
                        if (e.byteLength != t.byteLength || e.byteOffset != t.byteOffset) return !1;
                        (e = e.buffer), (t = t.buffer);
                    case "[object ArrayBuffer]":
                        return !(e.byteLength != t.byteLength || !d(new i(e), new i(t)));
                    case "[object Boolean]":
                    case "[object Date]":
                    case "[object Number]":
                        return a(+e, +t);
                    case "[object Error]":
                        return e.name == t.name && e.message == t.message;
                    case "[object RegExp]":
                    case "[object String]":
                        return e == t + "";
                    case "[object Map]":
                        var p = s;
                    case "[object Set]":
                        var h = 1 & n;
                        if ((p || (p = c), e.size != t.size && !h)) return !1;
                        var v = f.get(e);
                        if (v) return v == t;
                        (n |= 2), f.set(e, t);
                        var m = o(p(e), p(t), n, u, d, f);
                        return f.delete(e), m;
                    case "[object Symbol]":
                        if (l) return l.call(e) == l.call(t);
                }
                return !1;
            };
        },
        function (e, t, r) {
            var n = r(7).Uint8Array;
            e.exports = n;
        },
        function (e, t) {
            e.exports = function (e) {
                var t = -1,
                    r = Array(e.size);
                return (
                    e.forEach(function (e, n) {
                        r[++t] = [n, e];
                    }),
                    r
                );
            };
        },
        function (e, t) {
            e.exports = function (e) {
                var t = -1,
                    r = Array(e.size);
                return (
                    e.forEach(function (e) {
                        r[++t] = e;
                    }),
                    r
                );
            };
        },
        function (e, t, r) {
            var n = r(121),
                i = Object.prototype.hasOwnProperty;
            e.exports = function (e, t, r, a, o, s) {
                var c = 1 & r,
                    u = n(e),
                    l = u.length;
                if (l != n(t).length && !c) return !1;
                for (var d = l; d--;) {
                    var f = u[d];
                    if (!(c ? f in t : i.call(t, f))) return !1;
                }
                var p = s.get(e);
                if (p && s.get(t)) return p == t;
                var h = !0;
                s.set(e, t), s.set(t, e);
                for (var v = c; ++d < l;) {
                    var m = e[(f = u[d])],
                        g = t[f];
                    if (a) var b = c ? a(g, m, f, t, e, s) : a(m, g, f, e, t, s);
                    if (!(void 0 === b ? m === g || o(m, g, r, a, s) : b)) {
                        h = !1;
                        break;
                    }
                    v || (v = "constructor" == f);
                }
                if (h && !v) {
                    var y = e.constructor,
                        _ = t.constructor;
                    y != _ && "constructor" in e && "constructor" in t && !("function" == typeof y && y instanceof y && "function" == typeof _ && _ instanceof _) && (h = !1);
                }
                return s.delete(e), s.delete(t), h;
            };
        },
        function (e, t, r) {
            var n = r(122),
                i = r(124),
                a = r(29);
            e.exports = function (e) {
                return n(e, a, i);
            };
        },
        function (e, t, r) {
            var n = r(123),
                i = r(6);
            e.exports = function (e, t, r) {
                var a = t(e);
                return i(e) ? a : n(a, r(e));
            };
        },
        function (e, t) {
            e.exports = function (e, t) {
                for (var r = -1, n = t.length, i = e.length; ++r < n;) e[i + r] = t[r];
                return e;
            };
        },
        function (e, t, r) {
            var n = r(37),
                i = r(125),
                a = Object.prototype.propertyIsEnumerable,
                o = Object.getOwnPropertySymbols,
                s = o
                    ? function (e) {
                        return null == e
                            ? []
                            : ((e = Object(e)),
                                n(o(e), function (t) {
                                    return a.call(e, t);
                                }));
                    }
                    : i;
            e.exports = s;
        },
        function (e, t) {
            e.exports = function () {
                return [];
            };
        },
        function (e, t, r) {
            var n = r(127),
                i = r(33),
                a = r(128),
                o = r(129),
                s = r(130),
                c = r(14),
                u = r(51),
                l = u(n),
                d = u(i),
                f = u(a),
                p = u(o),
                h = u(s),
                v = c;
            ((n && "[object DataView]" != v(new n(new ArrayBuffer(1)))) ||
                (i && "[object Map]" != v(new i())) ||
                (a && "[object Promise]" != v(a.resolve())) ||
                (o && "[object Set]" != v(new o())) ||
                (s && "[object WeakMap]" != v(new s()))) &&
                (v = function (e) {
                    var t = c(e),
                        r = "[object Object]" == t ? e.constructor : void 0,
                        n = r ? u(r) : "";
                    if (n)
                        switch (n) {
                            case l:
                                return "[object DataView]";
                            case d:
                                return "[object Map]";
                            case f:
                                return "[object Promise]";
                            case p:
                                return "[object Set]";
                            case h:
                                return "[object WeakMap]";
                        }
                    return t;
                }),
                (e.exports = v);
        },
        function (e, t, r) {
            var n = r(12)(r(7), "DataView");
            e.exports = n;
        },
        function (e, t, r) {
            var n = r(12)(r(7), "Promise");
            e.exports = n;
        },
        function (e, t, r) {
            var n = r(12)(r(7), "Set");
            e.exports = n;
        },
        function (e, t, r) {
            var n = r(12)(r(7), "WeakMap");
            e.exports = n;
        },
        function (e, t, r) {
            var n = r(54),
                i = r(29);
            e.exports = function (e) {
                for (var t = i(e), r = t.length; r--;) {
                    var a = t[r],
                        o = e[a];
                    t[r] = [a, o, n(o)];
                }
                return t;
            };
        },
        function (e, t, r) {
            var n = r(52),
                i = r(133),
                a = r(139),
                o = r(35),
                s = r(54),
                c = r(55),
                u = r(26);
            e.exports = function (e, t) {
                return o(e) && s(t)
                    ? c(u(e), t)
                    : function (r) {
                        var o = i(r, e);
                        return void 0 === o && o === t ? a(r, e) : n(t, o, 3);
                    };
            };
        },
        function (e, t, r) {
            var n = r(56);
            e.exports = function (e, t, r) {
                var i = null == e ? void 0 : n(e, t);
                return void 0 === i ? r : i;
            };
        },
        function (e, t, r) {
            var n = r(135),
                i = /[^.[\]]+|\[(?:(-?\d+(?:\.\d+)?)|(["'])((?:(?!\2)[^\\]|\\.)*?)\2)\]|(?=(?:\.|\[\])(?:\.|\[\]|$))/g,
                a = /\\(\\)?/g,
                o = n(function (e) {
                    var t = [];
                    return (
                        46 === e.charCodeAt(0) && t.push(""),
                        e.replace(i, function (e, r, n, i) {
                            t.push(n ? i.replace(a, "$1") : r || e);
                        }),
                        t
                    );
                });
            e.exports = o;
        },
        function (e, t, r) {
            var n = r(136);
            e.exports = function (e) {
                var t = n(e, function (e) {
                    return 500 === r.size && r.clear(), e;
                }),
                    r = t.cache;
                return t;
            };
        },
        function (e, t, r) {
            var n = r(34);
            function i(e, t) {
                if ("function" != typeof e || (null != t && "function" != typeof t)) throw new TypeError("Expected a function");
                var r = function () {
                    var n = arguments,
                        i = t ? t.apply(this, n) : n[0],
                        a = r.cache;
                    if (a.has(i)) return a.get(i);
                    var o = e.apply(this, n);
                    return (r.cache = a.set(i, o) || a), o;
                };
                return (r.cache = new (i.Cache || n)()), r;
            }
            (i.Cache = n), (e.exports = i);
        },
        function (e, t, r) {
            var n = r(138);
            e.exports = function (e) {
                return null == e ? "" : n(e);
            };
        },
        function (e, t, r) {
            var n = r(20),
                i = r(58),
                a = r(6),
                o = r(25),
                s = n ? n.prototype : void 0,
                c = s ? s.toString : void 0;
            e.exports = function e(t) {
                if ("string" == typeof t) return t;
                if (a(t)) return i(t, e) + "";
                if (o(t)) return c ? c.call(t) : "";
                var r = t + "";
                return "0" == r && 1 / t == -1 / 0 ? "-0" : r;
            };
        },
        function (e, t, r) {
            var n = r(140),
                i = r(141);
            e.exports = function (e, t) {
                return null != e && i(e, t, n);
            };
        },
        function (e, t) {
            e.exports = function (e, t) {
                return null != e && t in Object(e);
            };
        },
        function (e, t, r) {
            var n = r(57),
                i = r(39),
                a = r(6),
                o = r(44),
                s = r(30),
                c = r(26);
            e.exports = function (e, t, r) {
                for (var u = -1, l = (t = n(t, e)).length, d = !1; ++u < l;) {
                    var f = c(t[u]);
                    if (!(d = null != e && r(e, f))) break;
                    e = e[f];
                }
                return d || ++u != l ? d : !!(l = null == e ? 0 : e.length) && s(l) && o(f, l) && (a(e) || i(e));
            };
        },
        function (e, t, r) {
            var n = r(143),
                i = r(144),
                a = r(35),
                o = r(26);
            e.exports = function (e) {
                return a(e) ? n(o(e)) : i(e);
            };
        },
        function (e, t) {
            e.exports = function (e) {
                return function (t) {
                    return null == t ? void 0 : t[e];
                };
            };
        },
        function (e, t, r) {
            var n = r(56);
            e.exports = function (e) {
                return function (t) {
                    return n(t, e);
                };
            };
        },
        function (e, t, r) {
            var n = r(58),
                i = r(48),
                a = r(146),
                o = r(147),
                s = r(46),
                c = r(148),
                u = r(59);
            e.exports = function (e, t, r) {
                var l = -1;
                t = n(t.length ? t : [u], s(i));
                var d = a(e, function (e, r, i) {
                    return {
                        criteria: n(t, function (t) {
                            return t(e);
                        }),
                        index: ++l,
                        value: e,
                    };
                });
                return o(d, function (e, t) {
                    return c(e, t, r);
                });
            };
        },
        function (e, t, r) {
            var n = r(38),
                i = r(31);
            e.exports = function (e, t) {
                var r = -1,
                    a = i(e) ? Array(e.length) : [];
                return (
                    n(e, function (e, n, i) {
                        a[++r] = t(e, n, i);
                    }),
                    a
                );
            };
        },
        function (e, t) {
            e.exports = function (e, t) {
                var r = e.length;
                for (e.sort(t); r--;) e[r] = e[r].value;
                return e;
            };
        },
        function (e, t, r) {
            var n = r(149);
            e.exports = function (e, t, r) {
                for (var i = -1, a = e.criteria, o = t.criteria, s = a.length, c = r.length; ++i < s;) {
                    var u = n(a[i], o[i]);
                    if (u) return i >= c ? u : u * ("desc" == r[i] ? -1 : 1);
                }
                return e.index - t.index;
            };
        },
        function (e, t, r) {
            var n = r(25);
            e.exports = function (e, t) {
                if (e !== t) {
                    var r = void 0 !== e,
                        i = null === e,
                        a = e == e,
                        o = n(e),
                        s = void 0 !== t,
                        c = null === t,
                        u = t == t,
                        l = n(t);
                    if ((!c && !l && !o && e > t) || (o && s && u && !c && !l) || (i && s && u) || (!r && u) || !a) return 1;
                    if ((!i && !o && !l && e < t) || (l && r && a && !i && !o) || (c && r && a) || (!s && a) || !u) return -1;
                }
                return 0;
            };
        },
        function (e, t) {
            e.exports = function (e) {
                return -1 !== Function.toString.call(e).indexOf("[native code]");
            };
        },
        function (e, t, r) {
            var n = r(28);
            function i() {
                if ("undefined" == typeof Reflect || !Reflect.construct) return !1;
                if (Reflect.construct.sham) return !1;
                if ("function" == typeof Proxy) return !0;
                try {
                    return Date.prototype.toString.call(Reflect.construct(Date, [], function () { })), !0;
                } catch (e) {
                    return !1;
                }
            }
            function a(t, r, o) {
                return (
                    i()
                        ? (e.exports = a = Reflect.construct)
                        : (e.exports = a = function (e, t, r) {
                            var i = [null];
                            i.push.apply(i, t);
                            var a = new (Function.bind.apply(e, i))();
                            return r && n(a, r.prototype), a;
                        }),
                    a.apply(null, arguments)
                );
            }
            e.exports = a;
        },
        function (e, t, r) {
            "use strict";
            r.r(t);
            var n = r(16),
                i = r.n(n),
                a = r(0),
                o = r.n(a),
                s = r(1),
                c = r.n(s),
                u = r(3),
                l = r.n(u),
                d = r(9),
                f = r.n(d),
                p = r(8),
                h = r.n(p),
                v = r(17),
                m = r.n(v),
                g = r(4),
                b = r.n(g),
                y = r(10),
                _ = r.n(y),
                k = r(13),
                w = r.n(k),
                M = r(11),
                S = r.n(M),
                x = r(62),
                O = r(18),
                T = r.n(O),
                A = r(19),
                j = r.n(A),
                C = "new",
                P = "loading",
                F = "joining-meeting",
                E = "joined-meeting",
                L = "left-meeting",
                N = "error",
                I = r(2);
            function R(e, t) {
                var r = Object.keys(e);
                if (Object.getOwnPropertySymbols) {
                    var n = Object.getOwnPropertySymbols(e);
                    t &&
                        (n = n.filter(function (t) {
                            return Object.getOwnPropertyDescriptor(e, t).enumerable;
                        })),
                        r.push.apply(r, n);
                }
                return r;
            }
            var B = (function (e) {
                function t() {
                    var e;
                    return l()(this, t), ((e = f()(this, h()(t).call(this)))._wrappedListeners = {}), (e._messageCallbacks = {}), e;
                }
                return (
                    _()(t, e),
                    b()(t, [
                        {
                            key: "addListenerForMessagesFromCallMachine",
                            value: function (e, t, r) {
                                var n = this,
                                    i = function (i) {
                                        if (i.data && "iframe-call-message" === i.data.what && (!i.data.callFrameId || i.data.callFrameId === t) && (!i.data.from || "module" !== i.data.from)) {
                                            var a = i.data;
                                            if ((delete a.from, a.callbackStamp && n._messageCallbacks[a.callbackStamp])) {
                                                var o = a.callbackStamp;
                                                n._messageCallbacks[o].call(r, a), delete n._messageCallbacks[o];
                                            }
                                            delete a.what, delete a.callbackStamp, e.call(r, a);
                                        }
                                    };
                                (this._wrappedListeners[e] = i), window.addEventListener("message", i);
                            },
                        },
                        {
                            key: "addListenerForMessagesFromDailyJs",
                            value: function (e, t, r) {
                                var n = function (n) {
                                    if (!(!n.data || "iframe-call-message" !== n.data.what || !n.data.action || (n.data.from && "module" !== n.data.from) || (n.data.callFrameId && t && n.data.callFrameId !== t))) {
                                        var i = n.data;
                                        e.call(r, i);
                                    }
                                };
                                (this._wrappedListeners[e] = n), window.addEventListener("message", n);
                            },
                        },
                        {
                            key: "sendMessageToCallMachine",
                            value: function (e, t, r, n) {
                                var i = (function (e) {
                                    for (var t = 1; t < arguments.length; t++) {
                                        var r = null != arguments[t] ? arguments[t] : {};
                                        t % 2
                                            ? R(Object(r), !0).forEach(function (t) {
                                                w()(e, t, r[t]);
                                            })
                                            : Object.getOwnPropertyDescriptors
                                                ? Object.defineProperties(e, Object.getOwnPropertyDescriptors(r))
                                                : R(Object(r)).forEach(function (t) {
                                                    Object.defineProperty(e, t, Object.getOwnPropertyDescriptor(r, t));
                                                });
                                    }
                                    return e;
                                })({}, e);
                                if (((i.what = "iframe-call-message"), (i.from = "module"), (i.callFrameId = n), t)) {
                                    var a = Date.now();
                                    (this._messageCallbacks[a] = t), (i.callbackStamp = a);
                                }
                                (r ? r.contentWindow : window).postMessage(i, "*");
                            },
                        },
                        {
                            key: "sendMessageToDailyJs",
                            value: function (e, t, r) {
                                (e.what = "iframe-call-message"), (e.callFrameId = r), (e.from = "embedded"), (t ? window : window.parent).postMessage(e, "*");
                            },
                        },
                        {
                            key: "removeListener",
                            value: function (e) {
                                var t = this._wrappedListeners[e];
                                t && (window.removeEventListener("message", t), delete this._wrappedListeners[e]);
                            },
                        },
                    ]),
                    t
                );
            })(r(27).a),
                D = r(60),
                V = r(61),
                U = r.n(V),
                z = r(5);
            var q = (function () {
                function e() {
                    l()(this, e), (this._currentLoad = null);
                }
                return (
                    b()(e, [
                        {
                            key: "load",
                            value: function (e, t, r, n) {
                                if (this.loaded) return window._dailyCallObjectSetup(t), void r(!0);
                                !(function (e) {
                                    window._dailyConfig || (window._dailyConfig = {}), (window._dailyConfig.callFrameId = e);
                                })(t),
                                    this._currentLoad && this._currentLoad.cancel(),
                                    (this._currentLoad = new J(
                                        e,
                                        t,
                                        function () {
                                            r(!1);
                                        },
                                        n
                                    )),
                                    this._currentLoad.start();
                            },
                        },
                        {
                            key: "cancel",
                            value: function () {
                                this._currentLoad && this._currentLoad.cancel();
                            },
                        },
                        {
                            key: "loaded",
                            get: function () {
                                return this._currentLoad && this._currentLoad.succeeded;
                            },
                        },
                    ]),
                    e
                );
            })(),
                J = (function () {
                    function e(t, r, n, i) {
                        l()(this, e), (this._attemptsRemaining = 3), (this._currentAttempt = null), (this._meetingOrBaseUrl = t), (this._callFrameId = r), (this._successCallback = n), (this._failureCallback = i);
                    }
                    return (
                        b()(e, [
                            {
                                key: "start",
                                value: function () {
                                    var e = this;
                                    if (!this._currentAttempt) {
                                        (this._currentAttempt = new G(this._meetingOrBaseUrl, this._callFrameId, this._successCallback, function t(r) {
                                            e._currentAttempt.cancelled ||
                                                (e._attemptsRemaining--,
                                                    e._failureCallback(r, e._attemptsRemaining > 0),
                                                    e._attemptsRemaining <= 0 ||
                                                    setTimeout(function () {
                                                        e._currentAttempt.cancelled || ((e._currentAttempt = new G(e._meetingOrBaseUrl, e._callFrameId, e._successCallback, t)), e._currentAttempt.start());
                                                    }, 3e3));
                                        })),
                                            this._currentAttempt.start();
                                    }
                                },
                            },
                            {
                                key: "cancel",
                                value: function () {
                                    this._currentAttempt && this._currentAttempt.cancel();
                                },
                            },
                            {
                                key: "cancelled",
                                get: function () {
                                    return this._currentAttempt && this._currentAttempt.cancelled;
                                },
                            },
                            {
                                key: "succeeded",
                                get: function () {
                                    return this._currentAttempt && this._currentAttempt.succeeded;
                                },
                            },
                        ]),
                        e
                    );
                })(),
                W = (function (e) {
                    function t() {
                        return l()(this, t), f()(this, h()(t).apply(this, arguments));
                    }
                    return _()(t, e), t;
                })(U()(Error)),
                G = (function () {
                    function e(t, r, n, i) {
                        l()(this, e),
                            (this.cancelled = !1),
                            (this.succeeded = !1),
                            (this._networkTimedOut = !1),
                            (this._networkTimeout = null),
                            (this._iosCache = "undefined" != typeof iOSCallObjectBundleCache && iOSCallObjectBundleCache),
                            (this._refetchHeaders = null),
                            (this._meetingOrBaseUrl = t),
                            (this._callFrameId = r),
                            (this._successCallback = n),
                            (this._failureCallback = i);
                    }
                    var t, r, n, i;
                    return (
                        b()(e, [
                            {
                                key: "start",
                                value:
                                    ((i = c()(
                                        o.a.mark(function e() {
                                            var t;
                                            return o.a.wrap(
                                                function (e) {
                                                    for (; ;)
                                                        switch ((e.prev = e.next)) {
                                                            case 0:
                                                                return (t = Object(z.a)(this._meetingOrBaseUrl)), (e.next = 3), this._tryLoadFromIOSCache(t);
                                                            case 3:
                                                                !e.sent && this._loadFromNetwork(t);
                                                            case 5:
                                                            case "end":
                                                                return e.stop();
                                                        }
                                                },
                                                e,
                                                this
                                            );
                                        })
                                    )),
                                        function () {
                                            return i.apply(this, arguments);
                                        }),
                            },
                            {
                                key: "cancel",
                                value: function () {
                                    clearTimeout(this._networkTimeout), (this.cancelled = !0);
                                },
                            },
                            {
                                key: "_tryLoadFromIOSCache",
                                value:
                                    ((n = c()(
                                        o.a.mark(function e(t) {
                                            var r;
                                            return o.a.wrap(
                                                function (e) {
                                                    for (; ;)
                                                        switch ((e.prev = e.next)) {
                                                            case 0:
                                                                if (this._iosCache) {
                                                                    e.next = 2;
                                                                    break;
                                                                }
                                                                return e.abrupt("return", !1);
                                                            case 2:
                                                                return (e.prev = 2), (e.next = 5), this._iosCache.get(t);
                                                            case 5:
                                                                if (((r = e.sent), !this.cancelled)) {
                                                                    e.next = 8;
                                                                    break;
                                                                }
                                                                return e.abrupt("return", !0);
                                                            case 8:
                                                                if (r) {
                                                                    e.next = 10;
                                                                    break;
                                                                }
                                                                return e.abrupt("return", !1);
                                                            case 10:
                                                                if (r.code) {
                                                                    e.next = 13;
                                                                    break;
                                                                }
                                                                return (this._refetchHeaders = r.refetchHeaders), e.abrupt("return", !1);
                                                            case 13:
                                                                return Function('"use strict";' + r.code)(), (this.succeeded = !0), this._successCallback(), e.abrupt("return", !0);
                                                            case 19:
                                                                return (e.prev = 19), (e.t0 = e.catch(2)), e.abrupt("return", !1);
                                                            case 22:
                                                            case "end":
                                                                return e.stop();
                                                        }
                                                },
                                                e,
                                                this,
                                                [[2, 19]]
                                            );
                                        })
                                    )),
                                        function (e) {
                                            return n.apply(this, arguments);
                                        }),
                            },
                            {
                                key: "_loadFromNetwork",
                                value:
                                    ((r = c()(
                                        o.a.mark(function e(t) {
                                            var r,
                                                n,
                                                i,
                                                a = this;
                                            return o.a.wrap(
                                                function (e) {
                                                    for (; ;)
                                                        switch ((e.prev = e.next)) {
                                                            case 0:
                                                                return (
                                                                    (this._networkTimeout = setTimeout(function () {
                                                                        (a._networkTimedOut = !0), a._failureCallback("Timed out (>".concat(2e4, " ms) when loading call object bundle ").concat(t));
                                                                    }, 2e4)),
                                                                    (e.prev = 1),
                                                                    (r = this._refetchHeaders ? { headers: this._refetchHeaders } : {}),
                                                                    (e.next = 5),
                                                                    fetch(t, r)
                                                                );
                                                            case 5:
                                                                if (((n = e.sent), clearTimeout(this._networkTimeout), !this.cancelled && !this._networkTimedOut)) {
                                                                    e.next = 9;
                                                                    break;
                                                                }
                                                                throw new W();
                                                            case 9:
                                                                return (e.next = 11), this._getBundleCodeFromResponse(t, n);
                                                            case 11:
                                                                if (((i = e.sent), !this.cancelled)) {
                                                                    e.next = 14;
                                                                    break;
                                                                }
                                                                throw new W();
                                                            case 14:
                                                                Function('"use strict";' + i)(), this._iosCache && this._iosCache.set(t, i, n.headers), (this.succeeded = !0), this._successCallback(), (e.next = 26);
                                                                break;
                                                            case 20:
                                                                if (((e.prev = 20), (e.t0 = e.catch(1)), clearTimeout(this._networkTimeout), !(e.t0 instanceof W || this.cancelled || this._networkTimedOut))) {
                                                                    e.next = 25;
                                                                    break;
                                                                }
                                                                return e.abrupt("return");
                                                            case 25:
                                                                this._failureCallback("Failed to load call object bundle ".concat(t, ": ").concat(e.t0));
                                                            case 26:
                                                            case "end":
                                                                return e.stop();
                                                        }
                                                },
                                                e,
                                                this,
                                                [[1, 20]]
                                            );
                                        })
                                    )),
                                        function (e) {
                                            return r.apply(this, arguments);
                                        }),
                            },
                            {
                                key: "_getBundleCodeFromResponse",
                                value:
                                    ((t = c()(
                                        o.a.mark(function e(t, r) {
                                            var n;
                                            return o.a.wrap(
                                                function (e) {
                                                    for (; ;)
                                                        switch ((e.prev = e.next)) {
                                                            case 0:
                                                                if (!r.ok) {
                                                                    e.next = 4;
                                                                    break;
                                                                }
                                                                return (e.next = 3), r.text();
                                                            case 3:
                                                                return e.abrupt("return", e.sent);
                                                            case 4:
                                                                if (!this._iosCache || 304 !== r.status) {
                                                                    e.next = 9;
                                                                    break;
                                                                }
                                                                return (e.next = 7), this._iosCache.renew(t, r.headers);
                                                            case 7:
                                                                return (n = e.sent), e.abrupt("return", n.code);
                                                            case 9:
                                                                throw new Error("Received ".concat(r.status, " response"));
                                                            case 10:
                                                            case "end":
                                                                return e.stop();
                                                        }
                                                },
                                                e,
                                                this
                                            );
                                        })
                                    )),
                                        function (e, r) {
                                            return t.apply(this, arguments);
                                        }),
                            },
                        ]),
                        e
                    );
                })(),
                H = function (e, t, r) {
                    return Q(e, e.local, t, r);
                },
                Q = function (e, t, r, n) {
                    return (
                        !!t &&
                        (!(t.public.subscribedTracks && !t.public.subscribedTracks.ALL) ||
                            (!!t.public.subscribedTracks[r] && (void 0 !== t.public.subscribedTracks[r].ALL ? t.public.subscribedTracks[r].ALL : t.public.subscribedTracks[r][n])))
                    );
                };
            function K(e, t) {
                var r = Object.keys(e);
                if (Object.getOwnPropertySymbols) {
                    var n = Object.getOwnPropertySymbols(e);
                    t &&
                        (n = n.filter(function (t) {
                            return Object.getOwnPropertyDescriptor(e, t).enumerable;
                        })),
                        r.push.apply(r, n);
                }
                return r;
            }
            function Y(e) {
                for (var t = 1; t < arguments.length; t++) {
                    var r = null != arguments[t] ? arguments[t] : {};
                    t % 2
                        ? K(Object(r), !0).forEach(function (t) {
                            w()(e, t, r[t]);
                        })
                        : Object.getOwnPropertyDescriptors
                            ? Object.defineProperties(e, Object.getOwnPropertyDescriptors(r))
                            : K(Object(r)).forEach(function (t) {
                                Object.defineProperty(e, t, Object.getOwnPropertyDescriptor(r, t));
                            });
                }
                return e;
            }
            r.d(t, "default", function () {
                return re;
            }),
                r.d(t, "DAILY_STATE_NEW", function () {
                    return C;
                }),
                r.d(t, "DAILY_STATE_JOINING", function () {
                    return F;
                }),
                r.d(t, "DAILY_STATE_JOINED", function () {
                    return E;
                }),
                r.d(t, "DAILY_STATE_LEFT", function () {
                    return L;
                }),
                r.d(t, "DAILY_STATE_ERROR", function () {
                    return N;
                }),
                r.d(t, "DAILY_EVENT_JOINING_MEETING", function () {
                    return "joining-meeting";
                }),
                r.d(t, "DAILY_EVENT_JOINED_MEETING", function () {
                    return "joined-meeting";
                }),
                r.d(t, "DAILY_EVENT_LEFT_MEETING", function () {
                    return "left-meeting";
                });
            var $ = "video",
                Z = "voice",
                X = { androidInCallNotification: { title: "string", subtitle: "string", iconName: "string", disableForCustomOverride: "boolean" }, disableAutoDeviceManagement: { audio: "boolean", video: "boolean" } },
                ee = {
                    url: {
                        validate: function (e) {
                            return "string" == typeof e;
                        },
                        help: "url should be a string",
                    },
                    baseUrl: {
                        validate: function (e) {
                            return "string" == typeof e;
                        },
                        help: "baseUrl should be a string",
                    },
                    token: {
                        validate: function (e) {
                            return "string" == typeof e;
                        },
                        help: "token should be a string",
                        queryString: "t",
                    },
                    dailyConfig: {
                        validate: function (e) {
                            return (
                                window._dailyConfig || (window._dailyConfig = {}),
                                (window._dailyConfig.experimentalGetUserMediaConstraintsModify = e.experimentalGetUserMediaConstraintsModify),
                                delete e.experimentalGetUserMediaConstraintsModify,
                                !0
                            );
                        },
                    },
                    reactNativeConfig: {
                        validate: function (e) {
                            return (function e(t, r) {
                                if (void 0 === r) return !1;
                                switch (i()(r)) {
                                    case "string":
                                        return i()(t) === r;
                                    case "object":
                                        if ("object" !== i()(t)) return !1;
                                        for (var n in t) if (!e(t[n], r[n])) return !1;
                                        return !0;
                                    default:
                                        return !1;
                                }
                            })(e, X);
                        },
                        help: "reactNativeConfig should look like ".concat(JSON.stringify(X), ", all fields optional"),
                    },
                    lang: {
                        validate: function (e) {
                            return ["de", "en-us", "en", "es", "fi", "fr", "it", "jp", "ka", "nl", "pl", "pt", "sv", "tr"].includes(e);
                        },
                        help: "language not supported. Options are: de, en-us, en, es, fi, fr, it, jp, ka, nl, pl, pt, sv, tr",
                    },
                    userName: !0,
                    showLeaveButton: !0,
                    showFullscreenButton: !0,
                    iframeStyle: !0,
                    customLayout: !0,
                    cssFile: !0,
                    cssText: !0,
                    bodyClass: !0,
                    videoSource: {
                        validate: function (e, t) {
                            return (t._preloadCache.videoDeviceId = e), !0;
                        },
                    },
                    audioSource: {
                        validate: function (e, t) {
                            return (t._preloadCache.audioDeviceId = e), !0;
                        },
                    },
                    subscribeToTracksAutomatically: {
                        validate: function (e, t) {
                            return (t._preloadCache.subscribeToTracksAutomatically = e), !0;
                        },
                    },
                    layout: {
                        validate: function (e) {
                            return "custom-v1" === e || "browser" === e || "none" === e;
                        },
                        help: 'layout may only be set to "custom-v1"',
                        queryString: "layout",
                    },
                    emb: { queryString: "emb" },
                    embHref: { queryString: "embHref" },
                    dailyJsVersion: { queryString: "dailyJsVersion" },
                },
                te = {
                    styles: {
                        validate: function (e) {
                            for (var t in e) if ("cam" !== t && "screen" !== t) return !1;
                            if (e.cam) for (var t in e.cam) if ("div" !== t && "video" !== t) return !1;
                            if (e.screen) for (var t in e.screen) if ("div" !== t && "video" !== t) return !1;
                            return !0;
                        },
                        help: "styles format should be a subset of: { cam: {div: {}, video: {}}, screen: {div: {}, video: {}} }",
                    },
                    setSubscribedTracks: {
                        validate: function (e, t, r) {
                            if (t._preloadCache.subscribeToTracksAutomatically) return !1;
                            var n = [!0, !1];
                            if ((!Object(I.b)() && n.push("avatar"), n.includes(e))) return !0;
                            for (var i in e) if ("audio" !== i && "video" !== i && "screenVideo" !== i && "screenAudio" !== i) return !1;
                            return !0;
                        },
                        help:
                            "setSubscribedTracks cannot be used when setSubscribeToTracksAutomatically is enabled, and should be of the form: " +
                            "true".concat(Object(I.b)() ? "" : " | 'avatar'", " | false | { [audio: true|false], [video: true|false], [screenVideo: true|false] }"),
                    },
                    setAudio: !0,
                    setVideo: !0,
                    eject: !0,
                },
                re = (function (e) {
                    function t(e) {
                        var r,
                            n = arguments.length > 1 && void 0 !== arguments[1] ? arguments[1] : {};
                        if (
                            (l()(this, t),
                                (r = f()(this, h()(t).call(this))),
                                w()(m()(r), "handleNativeAppActiveStateChange", function (e) {
                                    r.disableReactNativeAutoDeviceManagement("video") ||
                                        (e ? r.camUnmutedBeforeLosingNativeActiveState && r.setLocalVideo(!0) : ((r.camUnmutedBeforeLosingNativeActiveState = r.localVideo()), r.camUnmutedBeforeLosingNativeActiveState && r.setLocalVideo(!1)));
                                }),
                                w()(m()(r), "handleNativeAudioFocusChange", function (e) {
                                    r.disableReactNativeAutoDeviceManagement("audio") ||
                                        ((r._hasNativeAudioFocus = e),
                                            r.toggleParticipantAudioBasedOnNativeAudioFocus(),
                                            r._hasNativeAudioFocus ? r.micUnmutedBeforeLosingNativeAudioFocus && r.setLocalAudio(!0) : ((r.micUnmutedBeforeLosingNativeAudioFocus = r.localAudio()), r.setLocalAudio(!1)));
                                }),
                                (n.dailyJsVersion = "0.9.996"),
                                (r._iframe = e),
                                (r._callObjectMode = "none" === n.layout && !r._iframe),
                                (r._preloadCache = { subscribeToTracksAutomatically: !0, audioDeviceId: null, videoDeviceId: null, outputDeviceId: null }),
                                r._callObjectMode && (window._dailyPreloadCache = r._preloadCache),
                                r.validateProperties(n),
                                (r.properties = Y({}, n)),
                                (r._callObjectLoader = r._callObjectMode ? new q() : null),
                                (r._meetingState = C),
                                (r._isPreparingToJoin = !1),
                                (r._nativeInCallAudioMode = $),
                                (r._participants = {}),
                                (r._inputEventsOn = {}),
                                (r._network = { threshold: "good", quality: 100 }),
                                (r._activeSpeaker = {}),
                                (r._activeSpeakerMode = !1),
                                (r._callFrameId = Date.now() + Math.random().toString()),
                                (r._messageChannel = Object(I.b)() ? new D.a() : new B()),
                                r._iframe &&
                                (r._iframe.requestFullscreen
                                    ? r._iframe.addEventListener("fullscreenchange", function (e) {
                                        document.fullscreenElement === r._iframe
                                            ? (r.emit("fullscreen"), r.sendMessageToCallMachine({ action: "fullscreen" }))
                                            : (r.emit("exited-fullscreen"), r.sendMessageToCallMachine({ action: "exited-fullscreen" }));
                                    })
                                    : r._iframe.webkitRequestFullscreen &&
                                    r._iframe.addEventListener("webkitfullscreenchange", function (e) {
                                        document.webkitFullscreenElement === r._iframe
                                            ? (r.emit("fullscreen"), r.sendMessageToCallMachine({ action: "fullscreen" }))
                                            : (r.emit("exited-fullscreen"), r.sendMessageToCallMachine({ action: "exited-fullscreen" }));
                                    })),
                                Object(I.b)())
                        ) {
                            var i = r.nativeUtils();
                            (i.addAudioFocusChangeListener && i.removeAudioFocusChangeListener && i.addAppActiveStateChangeListener && i.removeAppActiveStateChangeListener) ||
                                console.warn("expected (add|remove)(AudioFocus|AppActiveState)ChangeListener to be available in React Native"),
                                (r._hasNativeAudioFocus = !0),
                                i.addAudioFocusChangeListener(r.handleNativeAudioFocusChange),
                                i.addAppActiveStateChangeListener(r.handleNativeAppActiveStateChange);
                        }
                        return r._messageChannel.addListenerForMessagesFromCallMachine(r.handleMessageFromCallMachine, r._callFrameId, m()(r)), r;
                    }
                    var r, n, i, a, s, u, d, p, v, g, y;
                    return (
                        _()(t, e),
                        b()(t, null, [
                            {
                                key: "supportedBrowser",
                                value: function () {
                                    return Object(I.a)();
                                },
                            },
                            {
                                key: "version",
                                value: function () {
                                    return "0.9.996";
                                },
                            },
                            {
                                key: "createCallObject",
                                value: function () {
                                    var e = arguments.length > 0 && void 0 !== arguments[0] ? arguments[0] : {};
                                    return (e.layout = "none"), new t(null, e);
                                },
                            },
                            {
                                key: "wrap",
                                value: function (e) {
                                    var r = arguments.length > 1 && void 0 !== arguments[1] ? arguments[1] : {};
                                    if ((ie(), !e || !e.contentWindow || "string" != typeof e.src)) throw new Error("DailyIframe::Wrap needs an iframe-like first argument");
                                    return r.layout || (r.customLayout ? (r.layout = "custom-v1") : (r.layout = "browser")), new t(e, r);
                                },
                            },
                            {
                                key: "createFrame",
                                value: function (e, r) {
                                    var n, i;
                                    ie(), e && r ? ((n = e), (i = r)) : e && e.append ? ((n = e), (i = {})) : ((n = document.body), (i = e || {}));
                                    var a = i.iframeStyle;
                                    a ||
                                        (a =
                                            n === document.body
                                                ? { position: "fixed", border: "1px solid black", backgroundColor: "white", width: "375px", height: "450px", right: "1em", bottom: "1em" }
                                                : { border: 0, width: "100%", height: "100%" });
                                    var o = document.createElement("iframe");
                                    window.navigator && window.navigator.userAgent.match(/Chrome\/61\./) ? (o.allow = "microphone, camera") : (o.allow = "microphone; camera; autoplay; display-capture"),
                                        (o.style.visibility = "hidden"),
                                        n.appendChild(o),
                                        (o.style.visibility = null),
                                        Object.keys(a).forEach(function (e) {
                                            return (o.style[e] = a[e]);
                                        }),
                                        i.layout || (i.customLayout ? (i.layout = "custom-v1") : (i.layout = "browser"));
                                    try {
                                        return new t(o, i);
                                    } catch (e) {
                                        throw (n.removeChild(o), e);
                                    }
                                },
                            },
                            {
                                key: "createTransparentFrame",
                                value: function () {
                                    var e = arguments.length > 0 && void 0 !== arguments[0] ? arguments[0] : {};
                                    ie();
                                    var r = document.createElement("iframe");
                                    return (
                                        (r.allow = "microphone; camera; autoplay; fullscree"),
                                        (r.style.cssText = "\n      position: fixed;\n      top: 0;\n      left: 0;\n      width: 100%;\n      height: 100%;\n      border: 0;\n      pointer-events: none;\n    "),
                                        document.body.appendChild(r),
                                        e.layout || (e.layout = "custom-v1"),
                                        t.wrap(r, e)
                                    );
                                },
                            },
                        ]),
                        b()(t, [
                            {
                                key: "destroy",
                                value:
                                    ((y = c()(
                                        o.a.mark(function e() {
                                            var t, r, n;
                                            return o.a.wrap(
                                                function (e) {
                                                    for (; ;)
                                                        switch ((e.prev = e.next)) {
                                                            case 0:
                                                                if (((e.prev = 0), ![E, P].includes(this._meetingState))) {
                                                                    e.next = 4;
                                                                    break;
                                                                }
                                                                return (e.next = 4), this.leave();
                                                            case 4:
                                                                e.next = 8;
                                                                break;
                                                            case 6:
                                                                (e.prev = 6), (e.t0 = e.catch(0));
                                                            case 8:
                                                                (t = this._iframe) && (r = t.parentElement) && r.removeChild(t),
                                                                    this._messageChannel.removeListener(this.handleMessageFromCallMachine),
                                                                    Object(I.b)() &&
                                                                    ((n = this.nativeUtils()).removeAudioFocusChangeListener(this.handleNativeAudioFocusChange),
                                                                        n.removeAppActiveStateChangeListener(this.handleNativeAppActiveStateChange));
                                                            case 12:
                                                            case "end":
                                                                return e.stop();
                                                        }
                                                },
                                                e,
                                                this,
                                                [[0, 6]]
                                            );
                                        })
                                    )),
                                        function () {
                                            return y.apply(this, arguments);
                                        }),
                            },
                            {
                                key: "loadCss",
                                value: function (e) {
                                    var t = e.bodyClass,
                                        r = e.cssFile,
                                        n = e.cssText;
                                    return ie(), this.sendMessageToCallMachine({ action: "load-css", cssFile: this.absoluteUrl(r), bodyClass: t, cssText: n }), this;
                                },
                            },
                            {
                                key: "iframe",
                                value: function () {
                                    return ie(), this._iframe;
                                },
                            },
                            {
                                key: "meetingState",
                                value: function () {
                                    return this._meetingState;
                                },
                            },
                            {
                                key: "participants",
                                value: function () {
                                    return this._participants;
                                },
                            },
                            {
                                key: "updateParticipant",
                                value: function (e, t) {
                                    if ((this._participants.local && this._participants.local.session_id === e && (e = "local"), e && t && this._participants[e])) {
                                        for (var r in t) {
                                            if (!te[r]) throw new Error("unrecognized updateParticipant property ".concat(r));
                                            if (te[r].validate && !te[r].validate(t[r], this, this._participants[e])) throw new Error(te[r].help);
                                        }
                                        this.sendMessageToCallMachine({ action: "update-participant", id: e, properties: t });
                                    }
                                    return this;
                                },
                            },
                            {
                                key: "updateParticipants",
                                value: function (e) {
                                    for (var t in e) this.updateParticipant(t, e[t]);
                                    return this;
                                },
                            },
                            {
                                key: "localAudio",
                                value: function () {
                                    return this._participants.local ? this._participants.local.audio : null;
                                },
                            },
                            {
                                key: "localVideo",
                                value: function () {
                                    return this._participants.local ? this._participants.local.video : null;
                                },
                            },
                            {
                                key: "setLocalAudio",
                                value: function (e) {
                                    return this.sendMessageToCallMachine({ action: "local-audio", state: e }), this;
                                },
                            },
                            {
                                key: "setLocalVideo",
                                value: function (e) {
                                    return this.sendMessageToCallMachine({ action: "local-video", state: e }), this;
                                },
                            },
                            {
                                key: "setBandwidth",
                                value: function (e) {
                                    var t = e.kbs,
                                        r = e.trackConstraints;
                                    return ie(), this.sendMessageToCallMachine({ action: "set-bandwidth", kbs: t, trackConstraints: r }), this;
                                },
                            },
                            {
                                key: "setDailyLang",
                                value: function (e) {
                                    return ie(), this.sendMessageToCallMachine({ action: "set-daily-lang", lang: e }), this;
                                },
                            },
                            {
                                key: "startCamera",
                                value: function () {
                                    var e = this,
                                        t = arguments.length > 0 && void 0 !== arguments[0] ? arguments[0] : {};
                                    return new Promise(
                                        (function () {
                                            var r = c()(
                                                o.a.mark(function r(n, i) {
                                                    var a;
                                                    return o.a.wrap(
                                                        function (r) {
                                                            for (; ;)
                                                                switch ((r.prev = r.next)) {
                                                                    case 0:
                                                                        if (
                                                                            ((a = function (e) {
                                                                                delete e.action, delete e.callbackStamp, n(e);
                                                                            }),
                                                                                !e.needsLoad())
                                                                        ) {
                                                                            r.next = 10;
                                                                            break;
                                                                        }
                                                                        return (r.prev = 2), (r.next = 5), e.load(t);
                                                                    case 5:
                                                                        r.next = 10;
                                                                        break;
                                                                    case 7:
                                                                        (r.prev = 7), (r.t0 = r.catch(2)), i(r.t0);
                                                                    case 10:
                                                                        e.sendMessageToCallMachine({ action: "start-camera", properties: ne(e.properties) }, a);
                                                                    case 11:
                                                                    case "end":
                                                                        return r.stop();
                                                                }
                                                        },
                                                        r,
                                                        null,
                                                        [[2, 7]]
                                                    );
                                                })
                                            );
                                            return function (e, t) {
                                                return r.apply(this, arguments);
                                            };
                                        })()
                                    );
                                },
                            },
                            {
                                key: "cycleCamera",
                                value: function () {
                                    var e = this;
                                    return new Promise(function (t, r) {
                                        e.sendMessageToCallMachine({ action: "cycle-camera" }, function (e) {
                                            t({ device: e.device });
                                        });
                                    });
                                },
                            },
                            {
                                key: "cycleMic",
                                value: function () {
                                    var e = this;
                                    return (
                                        ie(),
                                        new Promise(function (t, r) {
                                            e.sendMessageToCallMachine({ action: "cycle-mic" }, function (e) {
                                                t({ device: e.device });
                                            });
                                        })
                                    );
                                },
                            },
                            {
                                key: "getCameraFacingMode",
                                value: function () {
                                    var e = this;
                                    return (
                                        ae(),
                                        new Promise(function (t, r) {
                                            e.sendMessageToCallMachine({ action: "get-camera-facing-mode" }, function (e) {
                                                t(e.facingMode);
                                            });
                                        })
                                    );
                                },
                            },
                            {
                                key: "setInputDevices",
                                value: function (e) {
                                    var t = e.audioDeviceId,
                                        r = e.videoDeviceId,
                                        n = e.audioSource,
                                        i = e.videoSource;
                                    return (
                                        console.warn("setInputDevices() is deprecated: instead use setInputDevicesAsync(), which returns a Promise"),
                                        this.setInputDevicesAsync({ audioDeviceId: t, videoDeviceId: r, audioSource: n, videoSource: i }),
                                        this
                                    );
                                },
                            },
                            {
                                key: "setInputDevicesAsync",
                                value:
                                    ((g = c()(
                                        o.a.mark(function e(t) {
                                            var r,
                                                n,
                                                i,
                                                a,
                                                s = this;
                                            return o.a.wrap(
                                                function (e) {
                                                    for (; ;)
                                                        switch ((e.prev = e.next)) {
                                                            case 0:
                                                                if (
                                                                    ((r = t.audioDeviceId),
                                                                        (n = t.videoDeviceId),
                                                                        (i = t.audioSource),
                                                                        (a = t.videoSource),
                                                                        ie(),
                                                                        void 0 !== i && (r = i),
                                                                        void 0 !== a && (n = a),
                                                                        r && (this._preloadCache.audioDeviceId = r),
                                                                        n && (this._preloadCache.videoDeviceId = n),
                                                                        !this._callObjectMode || this._meetingState === E)
                                                                ) {
                                                                    e.next = 8;
                                                                    break;
                                                                }
                                                                return e.abrupt("return", {
                                                                    camera: { deviceId: this._preloadCache.videoDeviceId },
                                                                    mic: { deviceId: this._preloadCache.audioDeviceId },
                                                                    speaker: { deviceId: this._preloadCache.outputDeviceId },
                                                                });
                                                            case 8:
                                                                return (
                                                                    r instanceof MediaStreamTrack && (r = "daily-custom-track"),
                                                                    n instanceof MediaStreamTrack && (n = "daily-custom-track"),
                                                                    e.abrupt(
                                                                        "return",
                                                                        new Promise(function (e) {
                                                                            s.sendMessageToCallMachine({ action: "set-input-devices", audioDeviceId: r, videoDeviceId: n }, function (t) {
                                                                                delete t.action, delete t.callbackStamp, e(t);
                                                                            });
                                                                        })
                                                                    )
                                                                );
                                                            case 11:
                                                            case "end":
                                                                return e.stop();
                                                        }
                                                },
                                                e,
                                                this
                                            );
                                        })
                                    )),
                                        function (e) {
                                            return g.apply(this, arguments);
                                        }),
                            },
                            {
                                key: "setOutputDevice",
                                value: function (e) {
                                    var t = e.outputDeviceId;
                                    return (
                                        ie(), t && (this._preloadCache.outputDeviceId = t), this._callObjectMode && this._meetingState !== E ? this : (this.sendMessageToCallMachine({ action: "set-output-device", outputDeviceId: t }), this)
                                    );
                                },
                            },
                            {
                                key: "getInputDevices",
                                value:
                                    ((v = c()(
                                        o.a.mark(function e() {
                                            var t = this;
                                            return o.a.wrap(
                                                function (e) {
                                                    for (; ;)
                                                        switch ((e.prev = e.next)) {
                                                            case 0:
                                                                if ((ie(), !this._callObjectMode || this._meetingState === E)) {
                                                                    e.next = 3;
                                                                    break;
                                                                }
                                                                return e.abrupt("return", {
                                                                    camera: { deviceId: this._preloadCache.videoDeviceId },
                                                                    mic: { deviceId: this._preloadCache.audioDeviceId },
                                                                    speaker: { deviceId: this._preloadCache.outputDeviceId },
                                                                });
                                                            case 3:
                                                                return e.abrupt(
                                                                    "return",
                                                                    new Promise(function (e, r) {
                                                                        t.sendMessageToCallMachine({ action: "get-input-devices" }, function (t) {
                                                                            delete t.action, delete t.callbackStamp, e(t);
                                                                        });
                                                                    })
                                                                );
                                                            case 4:
                                                            case "end":
                                                                return e.stop();
                                                        }
                                                },
                                                e,
                                                this
                                            );
                                        })
                                    )),
                                        function () {
                                            return v.apply(this, arguments);
                                        }),
                            },
                            {
                                key: "nativeInCallAudioMode",
                                value: function () {
                                    return ae(), this._nativeInCallAudioMode;
                                },
                            },
                            {
                                key: "setNativeInCallAudioMode",
                                value: function (e) {
                                    if ((ae(), [$, Z].includes(e))) {
                                        if (e !== this._nativeInCallAudioMode)
                                            return (
                                                (this._nativeInCallAudioMode = e),
                                                !this.disableReactNativeAutoDeviceManagement("audio") && this.shouldDeviceUseInCallAudioMode(this._meetingState) && this.nativeUtils().setAudioMode(this._nativeInCallAudioMode),
                                                this
                                            );
                                    } else console.error("invalid in-call audio mode specified: ", e);
                                },
                            },
                            {
                                key: "load",
                                value:
                                    ((p = c()(
                                        o.a.mark(function e(t) {
                                            var r = this;
                                            return o.a.wrap(
                                                function (e) {
                                                    for (; ;)
                                                        switch ((e.prev = e.next)) {
                                                            case 0:
                                                                if (this.needsLoad()) {
                                                                    e.next = 2;
                                                                    break;
                                                                }
                                                                return e.abrupt("return");
                                                            case 2:
                                                                if ((t && (this.validateProperties(t), (this.properties = Y({}, this.properties, {}, t))), this._callObjectMode || this.properties.url)) {
                                                                    e.next = 5;
                                                                    break;
                                                                }
                                                                throw new Error("can't load iframe meeting because url property isn't set");
                                                            case 5:
                                                                this.updateMeetingState(P);
                                                                try {
                                                                    this.emit("loading", { action: "loading" });
                                                                } catch (e) {
                                                                    console.log("could not emit 'loading'");
                                                                }
                                                                if (!this._callObjectMode) {
                                                                    e.next = 11;
                                                                    break;
                                                                }
                                                                return e.abrupt(
                                                                    "return",
                                                                    new Promise(function (e, t) {
                                                                        r._callObjectLoader.cancel(),
                                                                            r._callObjectLoader.load(
                                                                                r.properties.url || r.properties.baseUrl,
                                                                                r._callFrameId,
                                                                                function (t) {
                                                                                    r.updateMeetingState("loaded"), t && r.emit("loaded", { action: "loaded" }), e();
                                                                                },
                                                                                function (e, n) {
                                                                                    r.emit("load-attempt-failed", { action: "load-attempt-failed", errorMsg: e }),
                                                                                        n || (r.updateMeetingState(N), r.emit("error", { action: "error", errorMsg: e }), t(e));
                                                                                }
                                                                            );
                                                                    })
                                                                );
                                                            case 11:
                                                                return (
                                                                    (this._iframe.src = this.assembleMeetingUrl()),
                                                                    e.abrupt(
                                                                        "return",
                                                                        new Promise(function (e, t) {
                                                                            r._loadedCallback = function (n) {
                                                                                if (r._meetingState !== N) {
                                                                                    for (var i in (r.updateMeetingState("loaded"), (r.properties.cssFile || r.properties.cssText) && r.loadCss(r.properties), r._inputEventsOn))
                                                                                        r.sendMessageToCallMachine({ action: "register-input-handler", on: i });
                                                                                    e();
                                                                                } else t(n);
                                                                            };
                                                                        })
                                                                    )
                                                                );
                                                            case 13:
                                                            case "end":
                                                                return e.stop();
                                                        }
                                                },
                                                e,
                                                this
                                            );
                                        })
                                    )),
                                        function (e) {
                                            return p.apply(this, arguments);
                                        }),
                            },
                            {
                                key: "join",
                                value:
                                    ((d = c()(
                                        o.a.mark(function e() {
                                            var t,
                                                r,
                                                n,
                                                i,
                                                a = this,
                                                s = arguments;
                                            return o.a.wrap(
                                                function (e) {
                                                    for (; ;)
                                                        switch ((e.prev = e.next)) {
                                                            case 0:
                                                                if (((t = s.length > 0 && void 0 !== s[0] ? s[0] : {}), (r = !1), !this.needsLoad())) {
                                                                    e.next = 15;
                                                                    break;
                                                                }
                                                                return this.updateIsPreparingToJoin(!0), (e.prev = 4), (e.next = 7), this.load(t);
                                                            case 7:
                                                                e.next = 13;
                                                                break;
                                                            case 9:
                                                                return (e.prev = 9), (e.t0 = e.catch(4)), this.updateIsPreparingToJoin(!1), e.abrupt("return", Promise.reject(e.t0));
                                                            case 13:
                                                                e.next = 33;
                                                                break;
                                                            case 15:
                                                                if (((r = !(!this.properties.cssFile && !this.properties.cssText)), !t.url)) {
                                                                    e.next = 31;
                                                                    break;
                                                                }
                                                                if (!this._callObjectMode) {
                                                                    e.next = 27;
                                                                    break;
                                                                }
                                                                if (((n = Object(z.a)(t.url)), (i = Object(z.a)(this.properties.url || this.properties.baseUrl)), n === i)) {
                                                                    e.next = 24;
                                                                    break;
                                                                }
                                                                return (
                                                                    console.error("error: in call object mode, can't change the daily.co call url after load() to one with a different bundle url (".concat(i, " -> ").concat(n, ")")),
                                                                    this.updateIsPreparingToJoin(!1),
                                                                    e.abrupt("return", Promise.reject())
                                                                );
                                                            case 24:
                                                                (this.properties.url = t.url), (e.next = 31);
                                                                break;
                                                            case 27:
                                                                if (!t.url || t.url === this.properties.url) {
                                                                    e.next = 31;
                                                                    break;
                                                                }
                                                                return (
                                                                    console.error("error: in iframe mode, can't change the daily.co call url after load() (".concat(this.properties.url, " -> ").concat(t.url, ")")),
                                                                    this.updateIsPreparingToJoin(!1),
                                                                    e.abrupt("return", Promise.reject())
                                                                );
                                                            case 31:
                                                                this.validateProperties(t), (this.properties = Y({}, this.properties, {}, t));
                                                            case 33:
                                                                if (this._meetingState !== E && this._meetingState !== F) {
                                                                    e.next = 37;
                                                                    break;
                                                                }
                                                                return console.warn("already joined meeting, call leave() before joining again"), this.updateIsPreparingToJoin(!1), e.abrupt("return");
                                                            case 37:
                                                                this.updateMeetingState(F, !1);
                                                                try {
                                                                    this.emit("joining-meeting", { action: "joining-meeting" });
                                                                } catch (e) {
                                                                    console.log("could not emit 'joining-meeting'");
                                                                }
                                                                return (
                                                                    this.sendMessageToCallMachine({ action: "join-meeting", properties: ne(this.properties), preloadCache: ne(this._preloadCache) }),
                                                                    e.abrupt(
                                                                        "return",
                                                                        new Promise(function (e, t) {
                                                                            a._joinedCallback = function (n, i) {
                                                                                if (a._meetingState !== N) {
                                                                                    if ((a.updateMeetingState(E), n))
                                                                                        for (var o in n) {
                                                                                            a.fixupParticipant(n[o]);
                                                                                            var s = n[o].local ? "local" : n[o].session_id;
                                                                                            a.matchParticipantTracks(s, n[o]), (a._participants[o] = Y({}, n[o])), a.toggleParticipantAudioBasedOnNativeAudioFocus();
                                                                                        }
                                                                                    r && a.loadCss(a.properties), e(n);
                                                                                } else t(i);
                                                                            };
                                                                        })
                                                                    )
                                                                );
                                                            case 41:
                                                            case "end":
                                                                return e.stop();
                                                        }
                                                },
                                                e,
                                                this,
                                                [[4, 9]]
                                            );
                                        })
                                    )),
                                        function () {
                                            return d.apply(this, arguments);
                                        }),
                            },
                            {
                                key: "leave",
                                value:
                                    ((u = c()(
                                        o.a.mark(function e() {
                                            var t = this;
                                            return o.a.wrap(function (e) {
                                                for (; ;)
                                                    switch ((e.prev = e.next)) {
                                                        case 0:
                                                            return e.abrupt(
                                                                "return",
                                                                new Promise(function (e, r) {
                                                                    var n = function () {
                                                                        t._iframe, t.updateMeetingState(L), (t._participants = {}), (t._activeSpeakerMode = !1), t._preloadCache;
                                                                        try {
                                                                            t.emit(L, { action: L });
                                                                        } catch (e) {
                                                                            console.log("could not emit 'left-meeting'");
                                                                        }
                                                                        e();
                                                                    };
                                                                    t._callObjectLoader && !t._callObjectLoader.loaded
                                                                        ? (t._callObjectLoader.cancel(), n())
                                                                        : t._meetingState === L || t._meetingState === N
                                                                            ? e()
                                                                            : t.sendMessageToCallMachine({ action: "leave-meeting" }, n);
                                                                })
                                                            );
                                                        case 1:
                                                        case "end":
                                                            return e.stop();
                                                    }
                                            }, e);
                                        })
                                    )),
                                        function () {
                                            return u.apply(this, arguments);
                                        }),
                            },
                            {
                                key: "startScreenShare",
                                value: function () {
                                    var e = arguments.length > 0 && void 0 !== arguments[0] ? arguments[0] : {};
                                    ie(), e.mediaStream && ((this._preloadCache.screenMediaStream = e.mediaStream), (e.mediaStream = "daily-custom-track")), this.sendMessageToCallMachine({ action: "local-screen-start", captureOptions: e });
                                },
                            },
                            {
                                key: "stopScreenShare",
                                value: function () {
                                    ie(), this.sendMessageToCallMachine({ action: "local-screen-stop" });
                                },
                            },
                            {
                                key: "startRecording",
                                value: function () {
                                    ie(), this.sendMessageToCallMachine({ action: "local-recording-start" });
                                },
                            },
                            {
                                key: "stopRecording",
                                value: function () {
                                    ie(), this.sendMessageToCallMachine({ action: "local-recording-stop" });
                                },
                            },
                            {
                                key: "startLiveStreaming",
                                value: function (e) {
                                    var t = e.rtmpUrl,
                                        r = e.width,
                                        n = void 0 === r ? 1920 : r,
                                        i = e.height,
                                        a = void 0 === i ? 1080 : i,
                                        o = e.backgroundColor,
                                        s = void 0 === o ? "0xff000000" : o;
                                    ie(), this.sendMessageToCallMachine({ action: "daily-method-start-live-streaming", rtmpUrl: t, width: n, height: a, backgroundColor: s });
                                },
                            },
                            {
                                key: "stopLiveStreaming",
                                value: function () {
                                    ie(), this.sendMessageToCallMachine({ action: "daily-method-stop-live-streaming" });
                                },
                            },
                            {
                                key: "getNetworkStats",
                                value: function () {
                                    var e = this;
                                    if (this._meetingState !== E) {
                                        return { stats: { latest: {} } };
                                    }
                                    return new Promise(function (t, r) {
                                        e.sendMessageToCallMachine({ action: "get-calc-stats" }, function (r) {
                                            t(Y({ stats: r.stats }, e._network));
                                        });
                                    });
                                },
                            },
                            {
                                key: "getActiveSpeaker",
                                value: function () {
                                    return ie(), this._activeSpeaker;
                                },
                            },
                            {
                                key: "setActiveSpeakerMode",
                                value: function (e) {
                                    return ie(), this.sendMessageToCallMachine({ action: "set-active-speaker-mode", enabled: e }), this;
                                },
                            },
                            {
                                key: "activeSpeakerMode",
                                value: function () {
                                    return ie(), this._activeSpeakerMode;
                                },
                            },
                            {
                                key: "subscribeToTracksAutomatically",
                                value: function () {
                                    return this._preloadCache.subscribeToTracksAutomatically;
                                },
                            },
                            {
                                key: "setSubscribeToTracksAutomatically",
                                value: function (e) {
                                    if (this._meetingState !== E) throw new Error("setSubscribeToTracksAutomatically() is only allowed while in a meeting");
                                    return (this._preloadCache.subscribeToTracksAutomatically = e), this.sendMessageToCallMachine({ action: "daily-method-subscribe-to-tracks-automatically", enabled: e }), this;
                                },
                            },
                            {
                                key: "enumerateDevices",
                                value:
                                    ((s = c()(
                                        o.a.mark(function e() {
                                            var t,
                                                r = this;
                                            return o.a.wrap(
                                                function (e) {
                                                    for (; ;)
                                                        switch ((e.prev = e.next)) {
                                                            case 0:
                                                                if ((ie(), !this._callObjectMode)) {
                                                                    e.next = 6;
                                                                    break;
                                                                }
                                                                return (e.next = 4), navigator.mediaDevices.enumerateDevices();
                                                            case 4:
                                                                return (
                                                                    (t = e.sent),
                                                                    e.abrupt("return", {
                                                                        devices: t.map(function (e) {
                                                                            return JSON.parse(JSON.stringify(e));
                                                                        }),
                                                                    })
                                                                );
                                                            case 6:
                                                                return e.abrupt(
                                                                    "return",
                                                                    new Promise(function (e, t) {
                                                                        r.sendMessageToCallMachine({ action: "enumerate-devices" }, function (t) {
                                                                            e({ devices: t.devices });
                                                                        });
                                                                    })
                                                                );
                                                            case 7:
                                                            case "end":
                                                                return e.stop();
                                                        }
                                                },
                                                e,
                                                this
                                            );
                                        })
                                    )),
                                        function () {
                                            return s.apply(this, arguments);
                                        }),
                            },
                            {
                                key: "sendAppMessage",
                                value: function (e) {
                                    var t = arguments.length > 1 && void 0 !== arguments[1] ? arguments[1] : "*";
                                    if (JSON.stringify(e).length > 4096) throw new Error("Message data too large. Max size is 4096");
                                    return this.sendMessageToCallMachine({ action: "app-msg", data: e, to: t }), this;
                                },
                            },
                            {
                                key: "addFakeParticipant",
                                value: function (e) {
                                    return ie(), this.sendMessageToCallMachine(Y({ action: "add-fake-participant" }, e)), this;
                                },
                            },
                            {
                                key: "setShowNamesMode",
                                value: function (e) {
                                    return (
                                        ie(),
                                        e && "always" !== e && "never" !== e
                                            ? (console.error('setShowNamesMode argument should be "always", "never", or false'), this)
                                            : (this.sendMessageToCallMachine({ action: "set-show-names", mode: e }), this)
                                    );
                                },
                            },
                            {
                                key: "detectAllFaces",
                                value: function () {
                                    var e = this;
                                    return (
                                        ie(),
                                        new Promise(function (t, r) {
                                            e.sendMessageToCallMachine({ action: "detect-all-faces" }, function (e) {
                                                delete e.action, delete e.callbackStamp, t(e);
                                            });
                                        })
                                    );
                                },
                            },
                            {
                                key: "requestFullscreen",
                                value:
                                    ((a = c()(
                                        o.a.mark(function e() {
                                            return o.a.wrap(
                                                function (e) {
                                                    for (; ;)
                                                        switch ((e.prev = e.next)) {
                                                            case 0:
                                                                if ((ie(), this._iframe && !document.fullscreenElement)) {
                                                                    e.next = 3;
                                                                    break;
                                                                }
                                                                return e.abrupt("return");
                                                            case 3:
                                                                return (e.prev = 3), (e.next = 6), this._iframe.requestFullscreen;
                                                            case 6:
                                                                if (!e.sent) {
                                                                    e.next = 10;
                                                                    break;
                                                                }
                                                                this._iframe.requestFullscreen(), (e.next = 11);
                                                                break;
                                                            case 10:
                                                                this._iframe.webkitRequestFullscreen();
                                                            case 11:
                                                                e.next = 16;
                                                                break;
                                                            case 13:
                                                                (e.prev = 13), (e.t0 = e.catch(3)), console.log("could not make video call fullscreen", e.t0);
                                                            case 16:
                                                            case "end":
                                                                return e.stop();
                                                        }
                                                },
                                                e,
                                                this,
                                                [[3, 13]]
                                            );
                                        })
                                    )),
                                        function () {
                                            return a.apply(this, arguments);
                                        }),
                            },
                            {
                                key: "exitFullscreen",
                                value: function () {
                                    ie(), document.fullscreenElement ? document.exitFullscreen() : document.webkitFullscreenElement && document.webkitExitFullscreen();
                                },
                            },
                            {
                                key: "room",
                                value:
                                    ((i = c()(
                                        o.a.mark(function e() {
                                            var t = this;
                                            return o.a.wrap(
                                                function (e) {
                                                    for (; ;)
                                                        switch ((e.prev = e.next)) {
                                                            case 0:
                                                                if (this._meetingState === E) {
                                                                    e.next = 4;
                                                                    break;
                                                                }
                                                                if (!this.properties.url) {
                                                                    e.next = 3;
                                                                    break;
                                                                }
                                                                return e.abrupt("return", { roomUrlPendingJoin: this.properties.url });
                                                            case 3:
                                                                return e.abrupt("return", null);
                                                            case 4:
                                                                return e.abrupt(
                                                                    "return",
                                                                    new Promise(function (e, r) {
                                                                        t.sendMessageToCallMachine({ action: "lib-room-info" }, function (t) {
                                                                            delete t.action, delete t.callbackStamp, e(t);
                                                                        });
                                                                    })
                                                                );
                                                            case 5:
                                                            case "end":
                                                                return e.stop();
                                                        }
                                                },
                                                e,
                                                this
                                            );
                                        })
                                    )),
                                        function () {
                                            return i.apply(this, arguments);
                                        }),
                            },
                            {
                                key: "geo",
                                value:
                                    ((n = c()(
                                        o.a.mark(function e() {
                                            return o.a.wrap(function (e) {
                                                for (; ;)
                                                    switch ((e.prev = e.next)) {
                                                        case 0:
                                                            return e.abrupt(
                                                                "return",
                                                                new Promise(
                                                                    (function () {
                                                                        var e = c()(
                                                                            o.a.mark(function e(t, r) {
                                                                                var n, i;
                                                                                return o.a.wrap(
                                                                                    function (e) {
                                                                                        for (; ;)
                                                                                            switch ((e.prev = e.next)) {
                                                                                                case 0:
                                                                                                    return (e.prev = 0), (e.next = 4), fetch("https://gs.daily.co/_ks_/x-swsl/:");
                                                                                                case 4:
                                                                                                    return (n = e.sent), (e.next = 7), n.json();
                                                                                                case 7:
                                                                                                    (i = e.sent), t({ current: i.geo }), (e.next = 15);
                                                                                                    break;
                                                                                                case 11:
                                                                                                    (e.prev = 11), (e.t0 = e.catch(0)), console.error("geo lookup failed", e.t0), t({ current: "" });
                                                                                                case 15:
                                                                                                case "end":
                                                                                                    return e.stop();
                                                                                            }
                                                                                    },
                                                                                    e,
                                                                                    null,
                                                                                    [[0, 11]]
                                                                                );
                                                                            })
                                                                        );
                                                                        return function (t, r) {
                                                                            return e.apply(this, arguments);
                                                                        };
                                                                    })()
                                                                )
                                                            );
                                                        case 1:
                                                        case "end":
                                                            return e.stop();
                                                    }
                                            }, e);
                                        })
                                    )),
                                        function () {
                                            return n.apply(this, arguments);
                                        }),
                            },
                            {
                                key: "setNetworkTopology",
                                value:
                                    ((r = c()(
                                        o.a.mark(function e(t) {
                                            var r = this;
                                            return o.a.wrap(function (e) {
                                                for (; ;)
                                                    switch ((e.prev = e.next)) {
                                                        case 0:
                                                            return (
                                                                ie(),
                                                                e.abrupt(
                                                                    "return",
                                                                    new Promise(
                                                                        (function () {
                                                                            var e = c()(
                                                                                o.a.mark(function e(n, i) {
                                                                                    var a;
                                                                                    return o.a.wrap(function (e) {
                                                                                        for (; ;)
                                                                                            switch ((e.prev = e.next)) {
                                                                                                case 0:
                                                                                                    (a = function (e) {
                                                                                                        e.error ? i({ error: e.error }) : n({ workerId: e.workerId });
                                                                                                    }),
                                                                                                        r.sendMessageToCallMachine({ action: "set-network-topology", opts: t }, a);
                                                                                                case 2:
                                                                                                case "end":
                                                                                                    return e.stop();
                                                                                            }
                                                                                    }, e);
                                                                                })
                                                                            );
                                                                            return function (t, r) {
                                                                                return e.apply(this, arguments);
                                                                            };
                                                                        })()
                                                                    )
                                                                )
                                                            );
                                                        case 2:
                                                        case "end":
                                                            return e.stop();
                                                    }
                                            }, e);
                                        })
                                    )),
                                        function (e) {
                                            return r.apply(this, arguments);
                                        }),
                            },
                            {
                                key: "setPlayNewParticipantSound",
                                value: function (e) {
                                    if ((ie(), "number" != typeof e && !0 !== e && !1 !== e)) throw new Error("argument to setShouldPlayNewParticipantSound should be true, false, or a number, but is ".concat(e));
                                    this.sendMessageToCallMachine({ action: "daily-method-set-play-ding", arg: e });
                                },
                            },
                            {
                                key: "on",
                                value: function (e, t) {
                                    return (this._inputEventsOn[e] = {}), this.sendMessageToCallMachine({ action: "register-input-handler", on: e }), S.a.prototype.on.call(this, e, t);
                                },
                            },
                            {
                                key: "once",
                                value: function (e, t) {
                                    return (this._inputEventsOn[e] = {}), this.sendMessageToCallMachine({ action: "register-input-handler", on: e }), S.a.prototype.once.call(this, e, t);
                                },
                            },
                            {
                                key: "off",
                                value: function (e, t) {
                                    return delete this._inputEventsOn[e], this.sendMessageToCallMachine({ action: "register-input-handler", off: e }), S.a.prototype.off.call(this, e, t);
                                },
                            },
                            {
                                key: "validateProperties",
                                value: function (e) {
                                    for (var t in e) {
                                        if (!ee[t]) throw new Error("unrecognized property '".concat(t, "'"));
                                        if (ee[t].validate && !ee[t].validate(e[t], this)) throw new Error("property '".concat(t, "': ").concat(ee[t].help));
                                    }
                                },
                            },
                            {
                                key: "assembleMeetingUrl",
                                value: function () {
                                    var e = Y({}, this.properties, { emb: this._callFrameId, embHref: encodeURIComponent(window.location.href) }),
                                        t = e.url.match(/\?/) ? "&" : "?";
                                    return (
                                        e.url +
                                        t +
                                        Object.keys(ee)
                                            .filter(function (t) {
                                                return ee[t].queryString && void 0 !== e[t];
                                            })
                                            .map(function (t) {
                                                return "".concat(ee[t].queryString, "=").concat(e[t]);
                                            })
                                            .join("&")
                                    );
                                },
                            },
                            {
                                key: "needsLoad",
                                value: function () {
                                    return [C, P, L, N].includes(this._meetingState);
                                },
                            },
                            {
                                key: "sendMessageToCallMachine",
                                value: function (e, t) {
                                    this._messageChannel.sendMessageToCallMachine(e, t, this._iframe, this._callFrameId);
                                },
                            },
                            {
                                key: "handleMessageFromCallMachine",
                                value: function (e) {
                                    switch (e.action) {
                                        case "loaded":
                                            this._loadedCallback && (this._loadedCallback(), (this._loadedCallback = null));
                                            try {
                                                this.emit(e.action, e);
                                            } catch (t) {
                                                console.log("could not emit", e);
                                            }
                                            break;
                                        case "joined-meeting":
                                            this._joinedCallback && (this._joinedCallback(e.participants), (this._joinedCallback = null));
                                            try {
                                                this.emit(e.action, e);
                                            } catch (t) {
                                                console.log("could not emit", e);
                                            }
                                            break;
                                        case "participant-joined":
                                        case "participant-updated":
                                            if (this._meetingState === L) return;
                                            if ((this.fixupParticipant(e), e.participant && e.participant.session_id)) {
                                                var t = e.participant.local ? "local" : e.participant.session_id;
                                                this.matchParticipantTracks(t, e.participant);
                                                try {
                                                    this.maybeEventTrackStopped(this._participants[t], e.participant, "audioTrack"),
                                                        this.maybeEventTrackStopped(this._participants[t], e.participant, "videoTrack"),
                                                        this.maybeEventTrackStopped(this._participants[t], e.participant, "screenVideoTrack"),
                                                        this.maybeEventTrackStopped(this._participants[t], e.participant, "screenAudioTrack"),
                                                        this.maybeEventTrackStarted(this._participants[t], e.participant, "audioTrack"),
                                                        this.maybeEventTrackStarted(this._participants[t], e.participant, "videoTrack"),
                                                        this.maybeEventTrackStarted(this._participants[t], e.participant, "screenVideoTrack"),
                                                        this.maybeEventTrackStarted(this._participants[t], e.participant, "screenAudioTrack");
                                                } catch (e) {
                                                    console.error("track events error", e);
                                                }
                                                if (!this.compareEqualForParticipantUpdateEvent(e.participant, this._participants[t])) {
                                                    (this._participants[t] = Y({}, e.participant)), this.toggleParticipantAudioBasedOnNativeAudioFocus();
                                                    try {
                                                        this.emit(e.action, e);
                                                    } catch (t) {
                                                        console.log("could not emit", e);
                                                    }
                                                }
                                            }
                                            break;
                                        case "participant-left":
                                            if ((this.fixupParticipant(e), e.participant && e.participant.session_id)) {
                                                var r = this._participants[e.participant.session_id];
                                                r &&
                                                    (this.maybeEventTrackStopped(r, null, "audioTrack"),
                                                        this.maybeEventTrackStopped(r, null, "videoTrack"),
                                                        this.maybeEventTrackStopped(r, null, "screenVideoTrack"),
                                                        this.maybeEventTrackStopped(r, null, "screenAudioTrack")),
                                                    delete this._participants[e.participant.session_id];
                                                try {
                                                    this.emit(e.action, e);
                                                } catch (t) {
                                                    console.log("could not emit", e);
                                                }
                                            }
                                            break;
                                        case "error":
                                            this._iframe && (this._iframe.src = ""),
                                                this.updateMeetingState(N),
                                                (this._participants = {}),
                                                this._loadedCallback && (this._loadedCallback(e.errorMsg), (this._loadedCallback = null)),
                                                this._joinedCallback && (this._joinedCallback(null, e.errorMsg), (this._joinedCallback = null));
                                            try {
                                                this.emit(e.action, e);
                                            } catch (t) {
                                                console.log("could not emit", e);
                                            }
                                            break;
                                        case "left-meeting":
                                            this._meetingState !== N && this.updateMeetingState(L);
                                            try {
                                                this.emit(e.action, e);
                                            } catch (t) {
                                                console.log("could not emit", e);
                                            }
                                            break;
                                        case "input-event":
                                            var n = this._participants[e.session_id];
                                            n || (n = e.session_id === this._participants.local.session_id ? this._participants.local : {});
                                            try {
                                                this.emit(e.event.type, { action: e.event.type, event: e.event, participant: Y({}, n) });
                                            } catch (t) {
                                                console.log("could not emit", e);
                                            }
                                            break;
                                        case "network-quality-change":
                                            var i = e.threshold,
                                                a = e.quality;
                                            if (i !== this._network.threshold || a !== this._network.quality) {
                                                (this._network.quality = a), (this._network.threshold = i);
                                                try {
                                                    this.emit(e.action, e);
                                                } catch (t) {
                                                    console.log("could not emit", e);
                                                }
                                            }
                                            break;
                                        case "active-speaker-change":
                                            var o = e.activeSpeaker;
                                            if (this._activeSpeaker.peerId !== o.peerId) {
                                                this._activeSpeaker.peerId = o.peerId;
                                                try {
                                                    this.emit(e.action, { action: e.action, activeSpeaker: this._activeSpeaker });
                                                } catch (t) {
                                                    console.log("could not emit", e);
                                                }
                                            }
                                            break;
                                        case "active-speaker-mode-change":
                                            var s = e.enabled;
                                            if (this._activeSpeakerMode !== s) {
                                                this._activeSpeakerMode = s;
                                                try {
                                                    this.emit(e.action, { action: e.action, enabled: this._activeSpeakerMode });
                                                } catch (t) {
                                                    console.log("could not emit", e);
                                                }
                                            }
                                            break;
                                        case "recording-started":
                                        case "recording-stopped":
                                        case "recording-stats":
                                        case "recording-error":
                                        case "recording-upload-completed":
                                        case "started-camera":
                                        case "camera-error":
                                        case "app-message":
                                        case "local-screen-share-started":
                                        case "local-screen-share-stopped":
                                        case "network-connection":
                                        case "recording-data":
                                        case "live-streaming-started":
                                        case "live-streaming-stopped":
                                        case "live-streaming-error":
                                            try {
                                                this.emit(e.action, e);
                                            } catch (t) {
                                                console.log("could not emit", e, t);
                                            }
                                            break;
                                        case "request-fullscreen":
                                            this.requestFullscreen();
                                            break;
                                        case "request-exit-fullscreen":
                                            this.exitFullscreen();
                                    }
                                },
                            },
                            {
                                key: "fixupParticipant",
                                value: function (e) {
                                    var t = e.participant ? e.participant : e;
                                    t.id && ((t.owner = !!t.owner), (t.session_id = t.id), (t.user_name = t.name), (t.joined_at = t.joinedAt), delete t.id, delete t.name, delete t.joinedAt);
                                },
                            },
                            {
                                key: "matchParticipantTracks",
                                value: function (e, t) {
                                    if (this._callObjectMode) {
                                        var r = store.getState();
                                        if ("local" !== e) {
                                            var n = !0;
                                            try {
                                                var i = r.participants[t.session_id];
                                                i && i.public && i.public.rtcType && "peer-to-peer" === i.public.rtcType.impl && i.private && !["connected", "completed"].includes(i.private.peeringState) && (n = !1);
                                            } catch (e) {
                                                console.error(e);
                                            }
                                            if (!n) return (t.audio = !1), (t.audioTrack = !1), (t.video = !1), (t.videoTrack = !1), (t.screen = !1), void (t.screenTrack = !1);
                                            try {
                                                var a = r.streams,
                                                    o = this._participants[t.session_id];
                                                if (t.audio && H(r, t.session_id, "cam-audio")) {
                                                    var s = j()(
                                                        T()(a, function (e) {
                                                            return e.participantId === t.session_id && "cam" === e.type && e.pendingTrack && "audio" === e.pendingTrack.kind;
                                                        }),
                                                        "starttime",
                                                        "desc"
                                                    );
                                                    s &&
                                                        s[0] &&
                                                        s[0].pendingTrack &&
                                                        (o && o.audioTrack && o.audioTrack.id === s[0].pendingTrack.id ? (t.audioTrack = s[0].pendingTrack) : s[0].pendingTrack.muted || (t.audioTrack = s[0].pendingTrack)),
                                                        t.audioTrack || (t.audio = !1);
                                                }
                                                if (t.video && H(r, t.session_id, "cam-video")) {
                                                    var c = j()(
                                                        T()(a, function (e) {
                                                            return e.participantId === t.session_id && "cam" === e.type && e.pendingTrack && "video" === e.pendingTrack.kind;
                                                        }),
                                                        "starttime",
                                                        "desc"
                                                    );
                                                    c &&
                                                        c[0] &&
                                                        c[0].pendingTrack &&
                                                        (o && o.videoTrack && o.videoTrack.id === c[0].pendingTrack.id ? (t.videoTrack = c[0].pendingTrack) : c[0].pendingTrack.muted || (t.videoTrack = c[0].pendingTrack)),
                                                        t.videoTrack || (t.video = !1);
                                                }
                                                if (t.screen && H(r, t.session_id, "screen-audio")) {
                                                    var u = j()(
                                                        T()(a, function (e) {
                                                            return e.participantId === t.session_id && "screen" === e.type && e.pendingTrack && "audio" === e.pendingTrack.kind;
                                                        }),
                                                        "starttime",
                                                        "desc"
                                                    );
                                                    u &&
                                                        u[0] &&
                                                        u[0].pendingTrack &&
                                                        (o && o.screenAudioTrack && o.screenAudioTrack.id === u[0].pendingTrack.id
                                                            ? (t.screenAudioTrack = u[0].pendingTrack)
                                                            : u[0].pendingTrack.muted || (t.screenAudioTrack = u[0].pendingTrack));
                                                }
                                                if (t.screen && H(r, t.session_id, "screen-video")) {
                                                    var l = j()(
                                                        T()(a, function (e) {
                                                            return e.participantId === t.session_id && "screen" === e.type && e.pendingTrack && "video" === e.pendingTrack.kind;
                                                        }),
                                                        "starttime",
                                                        "desc"
                                                    );
                                                    l &&
                                                        l[0] &&
                                                        l[0].pendingTrack &&
                                                        (o && o.screenVideoTrack && o.screenVideoTrack.id === l[0].pendingTrack.id
                                                            ? (t.screenVideoTrack = l[0].pendingTrack)
                                                            : l[0].pendingTrack.muted || (t.screenVideoTrack = l[0].pendingTrack));
                                                }
                                                t.screenVideoTrack || t.screenAudioTrack || (t.screen = !1);
                                            } catch (e) {
                                                console.error("unexpected error matching up tracks", e);
                                            }
                                        } else {
                                            if (t.audio)
                                                try {
                                                    (t.audioTrack = r.local.streams.cam.stream.getAudioTracks()[0]), t.audioTrack || (t.audio = !1);
                                                } catch (e) { }
                                            if (t.video)
                                                try {
                                                    (t.videoTrack = r.local.streams.cam.stream.getVideoTracks()[0]), t.videoTrack || (t.video = !1);
                                                } catch (e) { }
                                            if (t.screen)
                                                try {
                                                    (t.screenVideoTrack = r.local.streams.screen.stream.getVideoTracks()[0]),
                                                        (t.screenAudioTrack = r.local.streams.screen.stream.getAudioTracks()[0]),
                                                        t.screenVideoTrack || t.screenAudioTrack || (t.screen = !1);
                                                } catch (e) { }
                                        }
                                    }
                                },
                            },
                            {
                                key: "maybeEventTrackStopped",
                                value: function (e, t, r) {
                                    if (e && ((e[r] && "ended" === e[r].readyState) || (e[r] && (!t || !t[r])) || (e[r] && e[r].id !== t[r].id)))
                                        try {
                                            this.emit("track-stopped", { action: "track-stopped", track: e[r], participant: t });
                                        } catch (e) {
                                            console.log("could not emit", e);
                                        }
                                },
                            },
                            {
                                key: "maybeEventTrackStarted",
                                value: function (e, t, r) {
                                    if ((t[r] && (!e || !e[r])) || (t[r] && "ended" === e[r].readyState) || (t[r] && t[r].id !== e[r].id))
                                        try {
                                            this.emit("track-started", { action: "track-started", track: t[r], participant: t });
                                        } catch (e) {
                                            console.log("could not emit", e);
                                        }
                                },
                            },
                            {
                                key: "compareEqualForParticipantUpdateEvent",
                                value: function (e, t) {
                                    return (
                                        !!Object(x.deepEqual)(e, t) &&
                                        (!e.videoTrack || !t.videoTrack || (e.videoTrack.id === t.videoTrack.id && e.videoTrack.muted === t.videoTrack.muted && e.videoTrack.enabled === t.videoTrack.enabled)) &&
                                        (!e.audioTrack || !t.audioTrack || (e.audioTrack.id === t.audioTrack.id && e.audioTrack.muted === t.audioTrack.muted && e.audioTrack.enabled === t.audioTrack.enabled))
                                    );
                                },
                            },
                            {
                                key: "nativeUtils",
                                value: function () {
                                    return Object(I.b)() ? ("undefined" == typeof DailyNativeUtils ? (console.warn("in React Native, DailyNativeUtils is expected to be available"), null) : DailyNativeUtils) : null;
                                },
                            },
                            {
                                key: "updateIsPreparingToJoin",
                                value: function (e) {
                                    this.updateMeetingState(this._meetingState, e);
                                },
                            },
                            {
                                key: "updateMeetingState",
                                value: function (e) {
                                    var t = arguments.length > 1 && void 0 !== arguments[1] ? arguments[1] : this._isPreparingToJoin;
                                    if (e !== this._meetingState || t !== this._isPreparingToJoin) {
                                        var r = this._meetingState,
                                            n = this._isPreparingToJoin;
                                        (this._meetingState = e), (this._isPreparingToJoin = t);
                                        var i = this.isMeetingPendingOrOngoing(r, n),
                                            a = this.isMeetingPendingOrOngoing(this._meetingState, this._isPreparingToJoin);
                                        i !== a && (this.updateKeepDeviceAwake(a), this.updateDeviceAudioMode(a), this.updateShowAndroidOngoingMeetingNotification(a), this.updateNoOpRecordingEnsuringBackgroundContinuity(a));
                                    }
                                },
                            },
                            {
                                key: "updateKeepDeviceAwake",
                                value: function (e) {
                                    Object(I.b)() && this.nativeUtils().setKeepDeviceAwake(e, this._callFrameId);
                                },
                            },
                            {
                                key: "updateDeviceAudioMode",
                                value: function (e) {
                                    if (Object(I.b)() && !this.disableReactNativeAutoDeviceManagement("audio")) {
                                        var t = e ? this._nativeInCallAudioMode : "idle";
                                        this.nativeUtils().setAudioMode(t);
                                    }
                                },
                            },
                            {
                                key: "updateShowAndroidOngoingMeetingNotification",
                                value: function (e) {
                                    if (Object(I.b)() && this.nativeUtils().setShowOngoingMeetingNotification) {
                                        var t, r, n, i;
                                        if (this.properties.reactNativeConfig && this.properties.reactNativeConfig.androidInCallNotification) {
                                            var a = this.properties.reactNativeConfig.androidInCallNotification;
                                            (t = a.title), (r = a.subtitle), (n = a.iconName), (i = a.disableForCustomOverride);
                                        }
                                        i && (e = !1), this.nativeUtils().setShowOngoingMeetingNotification(e, t, r, n, this._callFrameId);
                                    }
                                },
                            },
                            {
                                key: "updateNoOpRecordingEnsuringBackgroundContinuity",
                                value: function (e) {
                                    Object(I.b)() && this.nativeUtils().enableNoOpRecordingEnsuringBackgroundContinuity && this.nativeUtils().enableNoOpRecordingEnsuringBackgroundContinuity(e);
                                },
                            },
                            {
                                key: "isMeetingPendingOrOngoing",
                                value: function (e, t) {
                                    return [F, E].includes(e) || t;
                                },
                            },
                            {
                                key: "toggleParticipantAudioBasedOnNativeAudioFocus",
                                value: function () {
                                    if (Object(I.b)()) {
                                        var e = store.getState();
                                        for (var t in e.streams) {
                                            var r = e.streams[t];
                                            r && r.pendingTrack && "audio" === r.pendingTrack.kind && (r.pendingTrack.enabled = this._hasNativeAudioFocus);
                                        }
                                    }
                                },
                            },
                            {
                                key: "disableReactNativeAutoDeviceManagement",
                                value: function (e) {
                                    return this.properties.reactNativeConfig && this.properties.reactNativeConfig.disableAutoDeviceManagement && this.properties.reactNativeConfig.disableAutoDeviceManagement[e];
                                },
                            },
                            {
                                key: "absoluteUrl",
                                value: function (e) {
                                    if (void 0 !== e) {
                                        var t = document.createElement("a");
                                        return (t.href = e), t.href;
                                    }
                                },
                            },
                            {
                                key: "sayHello",
                                value: function () {
                                    var e = "hello, world.";
                                    return console.log(e), e;
                                },
                            },
                        ]),
                        t
                    );
                })(S.a);
            function ne(e) {
                var t = {};
                for (var r in e)
                    e[r] instanceof MediaStreamTrack
                        ? (t[r] = "daily-custom-track")
                        : "dailyConfig" === r
                            ? (e[r].modifyLocalSdpHook && (window._dailyConfig && (window._dailyConfig.modifyLocalSdpHook = e[r].modifyLocalSdpHook), delete e[r].modifyLocalSdpHook),
                                e[r].modifyRemoteSdpHook && (window._dailyConfig && (window._dailyConfig.modifyRemoteSdpHook = e[r].modifyRemoteSdpHook), delete e[r].modifyRemoteSdpHook),
                                (t[r] = e[r]))
                            : (t[r] = e[r]);
                return t;
            }
            function ie() {
                if (Object(I.b)()) throw new Error("This daily-js method is not currently supported in React Native");
            }
            function ae() {
                if (!Object(I.b)()) throw new Error("This daily-js method is only supported in React Native");
            }
        },
    ]);
});
