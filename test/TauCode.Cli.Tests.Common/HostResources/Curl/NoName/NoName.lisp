(defblock :name curl :is-top t
	(executor
		:description "Sends HTTP requests to hosts."
		:usage-samples (
			"curl -H 'x-my-header:somevalue' http://k.me"
			"curl https://google.com"))

	(opt :name pre-url-keys
		(alt
			(seq
				(multi-text
					:classes key
					:values "-H"
					:alias header
					:action key
					:allows-multiple t
				)
				(some-text
					:classes string
					:alias header-value
					:action value
					:description "Header to add to request"
					:doc-subst "header")
			)
		)
	)

	(idle :name drago :links pre-url-keys next)

	(some-text
		:name url
		:classes url
		:alias url
		:action argument
		:description "URL to send to"
		:doc-subst "url"
	)

	(opt
		(multi-text
			:classes key
			:values "-v" "--verbose"
			:alias verbose
			:action option
			:description "Verbose output"
		)
	)

	(end)
)
