// credit: https://www.codementor.io/codehakase/building-a-restful-api-with-golang-a6yivzqdo
// https://github.com/gorilla/mux

package main

import (
	"fmt"
	"log"
	"net/http"
	"strings"

	"github.com/gorilla/mux"
)

// main function to boot up everything
func main() {
	router := mux.NewRouter()

	router.StrictSlash(false)
	router.Use(loggingMiddleware)

	router.PathPrefix("/").Handler(handleAll())

	port := ":80"

	metaReader(router)
	fmt.Println("Listening On ", port)
	log.Fatal(http.ListenAndServe(port, router))
}

func handleAll() http.Handler {
	return http.HandlerFunc(func(w http.ResponseWriter, r *http.Request) {
		log.Println("Anything:", r.RequestURI)
		fmt.Fprintf(w, "Other\nQuery %v\nPath %v", r.URL.Query(), r.RequestURI)
	})
}

// middleware
func loggingMiddleware(next http.Handler) http.Handler {
	return http.HandlerFunc(func(w http.ResponseWriter, r *http.Request) {
		log.Println(r.Host)
		log.Println(r.RequestURI)
		log.Println(r.URL.Query())
		log.Println(mux.Vars(r))

		// Call the next handler, which can be another middleware in the chain, or the final handler.
		next.ServeHTTP(w, r)
	})
}

func metaReader(r *mux.Router) {
	// swagger like
	err := r.Walk(func(route *mux.Route, router *mux.Router, ancestors []*mux.Route) error {
		pathTemplate, err := route.GetPathTemplate()
		if err == nil {
			fmt.Println("ROUTE:", pathTemplate)
		}
		pathRegexp, err := route.GetPathRegexp()
		if err == nil {
			fmt.Println("Path regexp:", pathRegexp)
		}
		queriesTemplates, err := route.GetQueriesTemplates()
		if err == nil {
			fmt.Println("Queries templates:", strings.Join(queriesTemplates, ","))
		}
		queriesRegexps, err := route.GetQueriesRegexp()
		if err == nil {
			fmt.Println("Queries regexps:", strings.Join(queriesRegexps, ","))
		}
		methods, err := route.GetMethods()
		if err == nil {
			fmt.Println("Methods:", strings.Join(methods, ","))
		}
		fmt.Println()
		return nil
	})

	if err != nil {
		fmt.Println(err)
	}
}
