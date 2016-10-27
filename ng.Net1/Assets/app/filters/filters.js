app.filter('string2JsonFilter', function ($filter) {
    function string2JsonFilter(v) {
        //return $filter('json')(JSON.parse(v));
        try {
            //var o = $filter('json')(JSON.parse(v));

            // Handle non-exception-throwing cases:
            // Neither JSON.parse(false) or JSON.parse(1234) throw errors, hence the type-checking,
            // but... JSON.parse(null) returns 'null', and typeof null === "object", 
            // so we must check for that, too.
            return $filter('json')(JSON.parse(v));
            
        }
        catch (e) {
            return v;
        }
    }

    return function (v) {
        return string2JsonFilter(v);
    }
});
