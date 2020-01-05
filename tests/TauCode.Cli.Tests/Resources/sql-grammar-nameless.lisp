(defblock :name sql :is-top t
	(worker
		:verbs "sql"
		:doc "Dummy nameless SQL worker."
	(alt
		(key-value-pair
			:alias query
			:key-names "--query" "-q"
			:key-values (choice :classes term :values *)
			:is-single t)

		(key-value-pair
			:alias exec
			:key-names "--exec" "-e"
			:key-values (choice :classes term :values *)
			:is-single t)
	)
	(end)
)
