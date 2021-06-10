(defblock :name start-publisher :is-top t
	(executor
		:executor-name start-publisher
		:verb "start"
		:description "Starts the publisher."
		:usage-samples (
			"pub start"))

	(end)
)
