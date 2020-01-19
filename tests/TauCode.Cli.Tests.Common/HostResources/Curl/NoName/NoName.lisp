(defblock :name curl :is-top t
	(worker
		:doc "Sends HTTP requests to hosts."
		:usage-samples (
			"<todo>"
			"<todo>"))

	(opt :name pre-url-keys
		(alt
			(seq
				(multi-text
					:classes key
					:values "-H"
					:alias header
					:action key
				)
				(some-text
					:classes string
					:alias header-value
					:action value)
			)
		)
	)

	(idle :name drago :links pre-url-keys next)

	(some-text
		:name url
		:classes url
		:alias url
		:action argument
	)

	(opt
		(multi-text
			:classes key
			:values "-v" "--verbose"
			:alias verbose
			:action option
		)
	)

	(end)
)
