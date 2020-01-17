(defblock :name curl :is-top t
	(worker
		:doc "Sends HTTP requests to hosts."
		:usage-samples (
			"<todo>"
			"<todo>"))

	(some-text
		:name url
		:classes url
		:alias url
		:action argument
	)

	(end)
)
